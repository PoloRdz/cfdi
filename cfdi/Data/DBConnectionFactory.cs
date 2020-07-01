using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Data
{
    public class DBConnectionFactory
    {
        //Data Source=LVALDEZ\SQLEXPRESS;Initial Catalog=CFD19_GestionCFDi_V9999_R0;User ID=sa;Password=123456
        public static SqlConnection GetOpenConnection()
        {
            SqlConnection cnn = new SqlConnection("Data Source=LVALDEZ\\SQLEXPRESS;Initial Catalog=CFD19_GestionCFDi_V9999_R0;User ID=sa;Password=123456");
            cnn.Open();
            return cnn;
        }

    }
}
