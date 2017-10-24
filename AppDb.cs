using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKTicket
{
    public class AppDb : IDisposable
    {
        public readonly MySqlConnection Connection;

        public AppDb()
        {
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
