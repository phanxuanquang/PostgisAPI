using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostgisUltilities
{
    public class DatabaseManager
    {
        private NpgsqlConnection connection;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseManager"/> class with the specified connection parameters.
        /// </summary>
        /// <param name="server">The address of the database server.</param>
        /// <param name="port">The port number to connect to the database server.</param>
        /// <param name="database">The name of the database to connect to.</param>
        /// <param name="username">The username used for authenticating the connection.</param>
        /// <param name="password">The password used for authenticating the connection.</param>
        public DatabaseManager(string server, int port, string database, string username, string password)
        {
            string connectionString = $"Host={server};Port={port};Database={database};Username={username};Password={password}";
            connection = new NpgsqlConnection(connectionString);

        }
        [DllImport("kernel32")]
        private static extern bool AllocConsole();
        #region Private Executers

        /// <summary>
        /// Activates the PostGIS extension in the PostgreSQL database
        /// </summary>
        public void ActivatePostGIS()
        {
            string sqlCommand = "CREATE EXTENSION postgis";
            Execute(sqlCommand, "Cannot activate PostGIS extension");
        }
        private void Execute(string sqlCommand, string errorMessage)
        {
            if (connection != null)
            {
                connection.Open();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = sqlCommand;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("Connection is not intilized", "Cannot execute SQL command", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /// <summary>
        /// Insert a list of model item into PostGIS database
        /// </summary>
        /// <param name="modelItems">The model item list to be inserted</param>
        /// <param name="activateParallelization">Apply the parallelization (default value is True). Larger the model, higher the performance.</param>
        public void Insert(List<ModelItemDB> modelItems, bool activateParallelization = true)
        {
            string prefix = "INSERT INTO ModelItem (ModelID, ModelItemID, DisplayName, HierachyIndex, ParentHierachyIndex, Path, Color, Mesh, Matrix, AABB, Properties, LastModifiedTime, BatchedModelItemID) VALUES ";
            object lockObject = new object();

            if (connection != null)
            {
                connection.Open();

                try
                {
                    using (NpgsqlTransaction transaction = connection.BeginTransaction())
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.Transaction = transaction;

                        if (activateParallelization)
                        {
                            Parallel.ForEach(Partitioner.Create(0, modelItems.Count), range =>
                            {
                                StringBuilder batchCommand = new StringBuilder();

                                for (int i = range.Item1; i < range.Item2; i++)
                                {
                                    ModelItemDB modelItem = modelItems[i];

                                    string values = $"('{modelItem.ModelID}', '{modelItem.ModelItemID}', '{modelItem.DisplayName.Replace("'", "")}', {modelItem.HierarchyIndex}, {modelItem.ParentHierachyIndex}, '{modelItem.Path}', '{modelItem.Color}', '{modelItem.Mesh}', '{"{"}{String.Join(", ", modelItem.Matrix)}{"}"}', '{modelItem.AABB}', '{modelItem.Properties.Replace("'", "")}', '{modelItem.LastModifiedTime}', null)";

                                    batchCommand.AppendLine(prefix + values + ";");
                                }

                                lock (lockObject)
                                {
                                    cmd.CommandText = batchCommand.ToString();
                                    cmd.ExecuteNonQuery();
                                }
                            });
                        }
                        else
                        {
                            foreach (ModelItemDB modelItem in modelItems)
                            {
                                string values = $"('{modelItem.ModelID}', '{modelItem.ModelItemID}', '{modelItem.DisplayName.Replace("'", "")}', {modelItem.HierarchyIndex}, {modelItem.ParentHierachyIndex}, '{modelItem.Path}', '{modelItem.Color}', '{modelItem.Mesh}', '{"{"}{String.Join(", ", modelItem.Matrix)}{"}"}', '{modelItem.AABB}', '{modelItem.Properties.Replace("'", "")}', '{modelItem.LastModifiedTime}', null)";

                                cmd.CommandText = prefix + values + ";";
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Connection is not initialized", "Cannot execute SQL command", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
