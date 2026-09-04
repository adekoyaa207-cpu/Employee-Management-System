using Microsoft.Data.SqlClient;

namespace EmployeeManagementSystem.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using SqlConnection connection = GetConnection();

                connection.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}