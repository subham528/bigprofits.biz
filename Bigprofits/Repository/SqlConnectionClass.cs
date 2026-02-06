using Microsoft.Data.SqlClient;
using System.Data;

namespace Bigprofits.Repository
{
    public class SqlConnectionClass(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public SqlConnection GetOpenConnection()
        {
            SqlConnection connection = new(_connectionString);
            connection.Open();
            return connection;
        }

        public async Task<DataSet> FnRetriveByPro(string procedureName, List<SqlParameter> parameters)
        {
            var ds = new DataSet();
            try
            {
                using var connection = GetOpenConnection();
                using var command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange([.. parameters]);
                }
                using var dataAdapter = new SqlDataAdapter(command);
                await Task.Run(() => dataAdapter.Fill(ds));
                return ds;
            }
            catch (Exception)
            {
                // Handle exception
            }
            return ds;
        }

        public async Task<DataSet> FnRetriveByQuery(string query)
        {
            var ds = new DataSet();
            try
            {
                using var connection = GetOpenConnection();
                using var command = new SqlCommand(query, connection);
                using var dataAdapter = new SqlDataAdapter(command);
                await Task.Run(() => dataAdapter.Fill(ds));
            }
            catch (Exception)
            {
            }
            return ds;
        }

        public dynamic ConvertDataSetToJson(DataTable table)
        {
            var result = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var item = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    item[column.ColumnName] = row[column];
                }
                result.Add(item);
            }
            return result;
        }



    }
}
