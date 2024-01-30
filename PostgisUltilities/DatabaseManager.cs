using Npgsql;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        /// <param name="modelItems"></param>
        public void Insert(List<ModelItemDB> modelItems)
        {
            string commands = string.Empty;
            string prefix = "INSERT INTO ModelItem (ModelID, ModelItemID, DisplayName, HierachyIndex, ParentHierachyIndex, Path, Color, Mesh, Matrix, AABB, Properties, LastModifiedTime, BatchedModelItemID) VALUES ";

            foreach (ModelItemDB modelItem in modelItems)
            {
                string values = $"('{modelItem.ModelID}', '{modelItem.ModelItemID}', '{modelItem.DisplayName.Replace("'", "")}', {modelItem.HierarchyIndex}, {modelItem.ParentHierachyIndex}, '{modelItem.Path}', '{modelItem.Color}', '{modelItem.Mesh}', '{"{"}{String.Join(", ", modelItem.Matrix)}{"}"}', '{modelItem.AABB}', '{modelItem.Properties.Replace("'", "")}', '{modelItem.LastModifiedTime}', null); \n";
                string command = prefix + values;
                commands += command;
            }
            Execute(commands, $"Inserting data failed");
        }

        private string prefix = "INSERT INTO ModelItem (ModelID, ModelItemID, DisplayName, HierachyIndex, ParentHierachyIndex, Path, Color, Mesh, Matrix, AABB, Properties, LastModifiedTime, BatchedModelItemID) VALUES ";
        public void InsertByParallel(List<ModelItemDB> modelItems)
        {
            int totalItem = modelItems.Count;
            try
            {
                Action[] functions = {
                    () => InsertByRange(modelItems, 0, totalItem / 5),
                    () => InsertByRange(modelItems, totalItem / 5, totalItem / 5 * 2),
                    () => InsertByRange(modelItems, totalItem / 5 * 2, totalItem / 5 * 3),
                    () => InsertByRange(modelItems, totalItem / 5 * 3, totalItem / 5 * 4),
                    () => InsertByRange(modelItems, totalItem / 5 * 4, totalItem)
                };

                Parallel.ForEach(functions, function =>
                {
                    function();
                });

                MessageBox.Show("Insert successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public async void InsertByTask(List<ModelItemDB> modelItems)
        {
            int totalItem = modelItems.Count;

            try
            {
                Task t1 = InsertByRange_Task(modelItems, 0, totalItem / 5);
                Task t2 = InsertByRange_Task(modelItems, totalItem / 5, totalItem / 5 * 2);
                Task t3 = InsertByRange_Task(modelItems, totalItem / 5 * 2, totalItem / 5 * 3);
                Task t4 = InsertByRange_Task(modelItems, totalItem / 5 * 3, totalItem / 5 * 4);
                Task t5 = InsertByRange_Task(modelItems, totalItem / 5 * 4, totalItem);

                await Task.WhenAll(t1, t2, t3, t4, t5);
                MessageBox.Show("Insert successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InsertByRange(List<ModelItemDB> modelItems, int startIndex, int endIndex)
        {
            string commands = string.Empty;
            for (int i = startIndex; i < endIndex; i++)
            {
                string values = $"('{modelItems[i].ModelID}', '{modelItems[i].ModelItemID}', '{modelItems[i].DisplayName.Replace("'", "")}', {modelItems[i].HierarchyIndex}, {modelItems[i].ParentHierachyIndex}, '{modelItems[i].Path}', '{modelItems[i].Color}', '{modelItems[i].Mesh}', '{"{"}{String.Join(", ", modelItems[i].Matrix)}{"}"}', '{modelItems[i].AABB}', '{modelItems[i].Properties.Replace("'", "")}', '{modelItems[i].LastModifiedTime}', null); \n";
                string command = prefix + values;
                commands += command;
            }
            Execute(commands, $"Inserting data failed");
            commands = string.Empty;
        }
        private async Task InsertByRange_Task(List<ModelItemDB> modelItems, int startIndex, int endIndex)
        {
            string commands = string.Empty;
            for (int i = startIndex; i < endIndex; i++)
            {
                string values = $"('{modelItems[i].ModelID}', '{modelItems[i].ModelItemID}', '{modelItems[i].DisplayName.Replace("'", "")}', {modelItems[i].HierarchyIndex}, {modelItems[i].ParentHierachyIndex}, '{modelItems[i].Path}', '{modelItems[i].Color}', '{modelItems[i].Mesh}', '{"{"}{String.Join(", ", modelItems[i].Matrix)}{"}"}', '{modelItems[i].AABB}', '{modelItems[i].Properties.Replace("'", "")}', '{modelItems[i].LastModifiedTime}', null); \n";
                string command = prefix + values;
                commands += command;
            }
            Execute(commands, $"Inserting data failed");
            commands = string.Empty;
        }
    }
}
