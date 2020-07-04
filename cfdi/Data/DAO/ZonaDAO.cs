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
    public class ZonaDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<Zona> getZonas()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_ZONAS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            SqlDataReader reader = null;
            List<Zona> zonas = new List<Zona>();
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("No se han encontrado zonas");
                while (reader.Read())
                {
                    zonas.Add(getZonaFromReader(reader));
                }
                return zonas;
            }
            catch(Exception e)
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

        private Zona getZonaFromReader(SqlDataReader rdr)
        {
            Zona zona = new Zona();
            zona.idZona = rdr.GetInt32(0);
            zona.zona = rdr.GetString(1);
            zona.descripcion = rdr.GetString(2);
            zona.identificador = rdr.GetString(3);
            return zona;
        }
    }
}
