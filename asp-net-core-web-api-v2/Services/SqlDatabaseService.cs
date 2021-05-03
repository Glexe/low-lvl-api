using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_web_api_v2.Services
{
    public class SqlDatabaseService : IDatabaseService
    {
        private readonly IConfiguration _config;
        private string _connectionString;

        //Splitting init in two steps since we cannot pass args directly using DI and solution with factories goes a bit complicated
        #region Initialization
        public SqlDatabaseService(IConfiguration config)
        {
            _config = config;
        }

        public void Build(string connectionStringName = "pjatk") //using pjatk by default if no args passed
        {
            _connectionString = _config.GetConnectionString(connectionStringName);
        }
        #endregion

        //sends a sql command and returns a number of affected rows
        public async Task<int> SendCommandAsync(SqlCommand command)
        {
            using(var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                command.Connection = sqlConnection;
                return await command.ExecuteNonQueryAsync();
            }
        }

        /*using enumerable let us iterate over every record from SqlDataReader
         *had to do so, coz we cannot return dr as a collection, and also cannot return the dr itself, since connection gonna be closed in func and in that case dr instance would be useless
         */
        public async IAsyncEnumerable<IDataRecord> SendRequestAsync(SqlCommand request)
        {
            var sqlConnection = new SqlConnection(_connectionString);
            request.Connection = sqlConnection;
            await sqlConnection.OpenAsync();
            var reader = await request.ExecuteReaderAsync();

            while (reader.Read()) //here we are reading the next record
            {
                //here we r returning that record on demand as a next item of our sequence
                yield return reader;
            }

            sqlConnection.Dispose();
        }
    }
}
