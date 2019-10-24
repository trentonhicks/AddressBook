using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AddressBook
{
    class SQL
    {
        // SQL connection string
        private static SqlConnection _connectionString = new SqlConnection(@"Data Source=.; Initial Catalog=AddressBook2.0; Integrated Security=SSPI;");
    }
}
