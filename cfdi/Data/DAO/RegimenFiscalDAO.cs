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
    public class RegimenFiscalDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<RegimenFiscal> getRegimenesFiscales()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_REGIMENES_FISCALES", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            SqlDataReader reader = null;
            var uos = new List<RegimenFiscal>();
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("No se han encontrado regimenes fiscales");
                while (reader.Read())
                    uos.Add(GetRegimenFiscalFromReader(reader));
                return uos;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                reader.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        private RegimenFiscal GetRegimenFiscalFromReader(SqlDataReader rdr)
        {
            var rf = new RegimenFiscal
            {
                idRegimenFiscal = rdr.GetInt32(0),
                descripcion = rdr.GetString(1),
                personaFisica = rdr.GetBoolean(2),
                personaMoral = rdr.GetBoolean(3)
            };
            return rf;
        }
    }
}
