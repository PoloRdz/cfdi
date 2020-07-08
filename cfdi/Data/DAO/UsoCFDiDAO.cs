using cfdi.Exceptions;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Data.DAO
{
    public class UsoCFDiDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public List<UsoCFDi> getUsoCFDis()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USO_CFDIS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = null;
            List<UsoCFDi> usoCFDis = new List<UsoCFDi>();
            try
            {
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new NotFoundException("No se encontraron UsoCFDis");
                while (rdr.Read())
                    usoCFDis.Add(getUsoCFDiFromReader(rdr));
                return usoCFDis;
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        private UsoCFDi getUsoCFDiFromReader(SqlDataReader rdr)
        {
            UsoCFDi uso = new UsoCFDi
            {
                usoCFDi = rdr.GetString(0),
                descripcion = rdr.GetString(1),
                explicacion = rdr.GetString(2),
                fisica = rdr.GetBoolean(3),
                moral = rdr.GetBoolean(4)
            };
            return uso;
        }
    }
}
