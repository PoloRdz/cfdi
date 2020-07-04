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
    public class UnidadOperativaDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<UnidadOperativa> GetUnidadOperativasByZonaId(int id)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_UNIDAD_OPERATIVA_POR_ZONA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_ID_ZONA", id);
            SqlDataReader reader = null;
            List<UnidadOperativa> uos = new List<UnidadOperativa>();
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("No se han encontrado unidades operativas");
                while (reader.Read())
                    uos.Add(GetUnidadOperativaFromReader(reader));
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

        private UnidadOperativa GetUnidadOperativaFromReader(SqlDataReader rdr)
        {
            UnidadOperativa uo = new UnidadOperativa();
            uo.idUnidadOperativa = rdr.GetInt32(0);
            uo.unidadOperativa = rdr.GetString(1);
            uo.descripcion = rdr.GetString(2);
            uo.identificador = rdr.GetString(3);
            uo.telefono = rdr.GetString(4);
            uo.calle = rdr.GetString(5);
            uo.numeroExterior = rdr.GetString(6);
            uo.numeroInterior = rdr.GetString(7);
            uo.colonia = rdr.GetString(8);
            uo.poblacion = rdr.GetString(9);
            uo.codigoPostal = rdr.GetString(10);
            uo.municipio = rdr.GetString(11);
            uo.razonSocial = new RazonSocial();
            uo.razonSocial.idRazonSocial = rdr.GetInt32(12);
            uo.razonSocial.razonSocial = rdr.GetString(13);
            uo.razonSocial.rfc = rdr.GetString(14);
            return uo;
        }
    }
}
