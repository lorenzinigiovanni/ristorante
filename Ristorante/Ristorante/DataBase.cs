using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ristorante
{
    public class DataBase
    {
        private SQLiteConnection _dbConnection;

        /// <summary>
        /// Create a database connection
        /// </summary>
        /// <param name="path">The OS path of the database file</param>
        /// <returns>Return true if the operation is successful or false if it has failed</returns>
        public async Task<bool> DataBaseConnectionAsync(string path)
        {
            try
            {
                _dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3");
                await _dbConnection.OpenAsync();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Reads all long description from database
        /// </summary>
        /// <returns>Return an array of description strings</returns>
        public async Task<string[]> GetDescriptionsAsync()
        {
            try
            {
                var tmp = new List<string>();

                const string query = "SELECT description FROM Descriptions";
                var command = new SQLiteCommand(query, _dbConnection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    tmp.Add((string)reader["description"]);

                return tmp.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Reads all description for 16 charaters display use from database
        /// </summary>
        /// <returns>Return an array of description strings</returns>
        public async Task<string[]> GetDisplayDescriptionsAsync()
        {
            try
            {
                var tmp = new List<string>();

                const string query = "SELECT displayDescription FROM Descriptions";
                var command = new SQLiteCommand(query, _dbConnection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    tmp.Add((string)reader["displayDescription"]);

                return tmp.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Reads all description for printer use from database
        /// </summary>
        /// <returns>Return an array of description strings</returns>
        public async Task<string[]> GetPrinterDescriptionsAsync()
        {
            try
            {
                var tmp = new List<string>();

                const string query = "SELECT printerDescription FROM Descriptions";
                var command = new SQLiteCommand(query, _dbConnection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    tmp.Add((string)reader["printerDescription"]);

                return tmp.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Reads all plates' price from database
        /// </summary>
        /// <returns>Return an array of prices strings</returns>
        public async Task<string[]> GetPricesAsync()
        {
            try
            {
                var tmp = new List<string>();

                const string query = "SELECT price FROM Descriptions";
                var command = new SQLiteCommand(query, _dbConnection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    tmp.Add(Convert.ToString(reader["price"]));

                return tmp.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get the plates number of the specified order
        /// </summary>
        /// <param name="order">Order number</param>
        /// <returns>Return an array of plates number</returns>
        public async Task<int[]> GetOrderAsync(int order)
        {
            try
            {
                var tmp = new List<int>();

                for (var i = 0; i < 18; i++)
                {
                    var query = "SELECT plate" + i + " FROM Orders WHERE orderNumber = " + order;
                    var command = new SQLiteCommand(Convert.ToString(query), _dbConnection);
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                        tmp.Add(Convert.ToInt32(reader["plate" + i]));
                }

                return tmp.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get the number of the last order
        /// Use the return number + 1 for a free order
        /// </summary>
        /// <returns>Return the last used order number</returns>
        public async Task<int> GetLastOrderNumberAsync()
        {
            try
            {
                const string query = "SELECT orderNumber FROM Orders ORDER BY orderNumber DESC LIMIT 1";
                var command = new SQLiteCommand(query, _dbConnection);

                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Get the number of not executed orders
        /// </summary>
        /// <returns>Return the number of not executed orders</returns>
        public async Task<int> GetRemainingOrdersNumberAsync()
        {
            try
            {
                const string query = "SELECT count(executed) FROM Orders WHERE executed = 0";
                var command = new SQLiteCommand(query, _dbConnection);

                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Get the sum of a type of plate of not executed orders
        /// </summary>
        /// <param name="plate">The id of the plate to check for</param>
        /// <returns>Returns the sum of to do plate</returns>
        public async Task<int> GetOrdersToDoAsync(int plate)
        {
            try
            {
                var query = "SELECT ifnull(sum(plate" + plate;
                query += "), 0) FROM Orders WHERE executed = 0";
                var command = new SQLiteCommand(query, _dbConnection);

                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Get all the "orders" table
        /// </summary>
        /// <returns>Returns the whole orders table</returns>
        public async Task<DataSet> GetAllOrdersAsync()
        {
            try
            {
                const string query = "SELECT * FROM Orders";
                
                var dataSet = new DataSet();

                await Task.Run(() =>
                {
                    var command = new SQLiteCommand(query, _dbConnection);
                    var adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(dataSet);
                });

                return dataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Check if a order is already done
        /// </summary>
        /// <param name="order">Order number</param>
        /// <returns>Return a boolean (false = order done, true = order to do)</returns>
        public async Task<bool> IsOrderToDoAsync(int order)
        {
            try
            {
                var query = "SELECT count(executed) FROM Orders WHERE executed = 0 AND orderNumber =" + order;
                var command = new SQLiteCommand(query, _dbConnection);

                return Convert.ToBoolean(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Add a new order to the database table "orders"
        /// </summary>
        /// <param name="values">The number of plates in the order</param>
        /// <param name="order">The number of the order</param>
        public async Task AddOrderAsync(int[] values, int order)
        {
            try
            {
                var query = new StringBuilder();
                query.Append("INSERT INTO Orders (orderNumber");

                for (var i = 0; i < values.Length; i++)
                    query.Append(", plate" + i);

                query.Append(") VALUES (" + order);

                foreach (var t in values)
                    query.Append(", " + t);

                query.Append(")");

                var command = new SQLiteCommand(query.ToString(), _dbConnection);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Update a order record setting the executed flag to 1
        /// </summary>
        /// <param name="order">Order number</param>
        public async Task SetOrderDoAsync(int order)
        {
            try
            {
                var query = "UPDATE Orders SET executed = 1 WHERE orderNumber = " + order;
                var command = new SQLiteCommand(Convert.ToString(query), _dbConnection);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Clear all the "orders" table and autoincrement counter
        /// </summary>
        public async Task DataBaseResetAsync()
        {
            try
            {
                var command1 = new SQLiteCommand("DELETE FROM Orders", _dbConnection);
                var command2 = new SQLiteCommand("DELETE FROM sqlite_sequence", _dbConnection);

                var task1 = command1.ExecuteNonQueryAsync();
                var task2 = command2.ExecuteNonQueryAsync();

                await task1;
                await task2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}