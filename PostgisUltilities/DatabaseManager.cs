using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
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
        /// <param name="modelItems"></param>
        public void Insert(List<ModelItemDB> modelItems)
        {
            string commands = string.Empty;
            string prefix = "INSERT INTO ModelItem (ModelID, ModelItemID, DisplayName, HierachyIndex, ParentHierachyIndex, Path, Color, Mesh, Matrix, AABB, Properties, LastModifiedTime, BatchedModelItemID) VALUES ";

            foreach (ModelItemDB modelItem in modelItems)
            {
                string values = $"('ed974e1e-7ba0-4fb0-9a35-07fb6f4c7ead', '{modelItem.ModelItemID}', '{modelItem.DisplayName.Replace("'", "")}', {modelItem.HierarchyIndex}, {modelItem.ParentHierachyIndex}, '{modelItem.Path}', '{modelItem.Color}', '{modelItem.Mesh}', '{String.Format("{{0}}", modelItem.Matrix)}', '{modelItem.AABB}', '{modelItem.Properties.Replace("'", "")}', '{modelItem.LastModifiedTime}', null); \n";
                string command = prefix + values;
                commands += command;
            }
            Execute(commands, $"Inserting data failed");
        }

        public void BatchModelItems(List<ModelItemDB> modelItems, List<Guid> batchModelItemIDs)
        {
            string commands = string.Empty;
            string prefix = "UPDATE ModelItem SET BatchedModelItemID = ";
            for (int i = 0; i < modelItems.Count; i++)
            {
                string value = $"'{batchModelItemIDs[i]}' WHERE ModelItemID = '{modelItems[i].ModelItemID}';\n";
                string command = prefix + value;
                commands += command;
            }
            Execute(commands, $"Batch model items failed");
        }
    }
}
