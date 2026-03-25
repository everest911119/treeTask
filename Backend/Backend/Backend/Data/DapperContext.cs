using Microsoft.Data.SqlClient;
using System.Data;


namespace Backend.Data
{
    public class DapperContext
    {
        private readonly IConfiguration configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            _connectionString = configuration.GetConnectionString("Database");
        }

            public IDbConnection CreateConnection()
            {
                return new SqlConnection(_connectionString);
        }
    }
}
