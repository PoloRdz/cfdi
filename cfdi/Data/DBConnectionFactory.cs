﻿using System;
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
            SqlConnection cnn = new SqlConnection("Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=CFD19_GestionCFDi_V9999_R0;User ID=sa;Password=Amonos123");
            //SqlConnection cnn = new SqlConnection("Data Source=10.0.0.21\\SQLEXPRESS;Initial Catalog=CFD19_GestionCFDi_V9999_R0;User ID=sa;Password=JI-15186#KF");
            cnn.Open();
            return cnn;
        }

    }
}
