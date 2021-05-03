using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_web_api_v2.Services
{
    public interface IDatabaseService
    {
        public void Build(string connectionStringName = "pjatk");
        public Task<int> SendCommandAsync(SqlCommand command);
        public IAsyncEnumerable<IDataRecord> SendRequestAsync(SqlCommand request);
    }
}
