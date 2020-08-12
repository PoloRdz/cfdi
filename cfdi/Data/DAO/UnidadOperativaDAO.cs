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

        public List<UnidadOperativa> ObtenerUnidadOperativasPorZona(int idZona)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_UNIDAD_OPERATIVA_POR_ZONA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_ID_ZONA", idZona);
            SqlDataReader reader = null;
            List<UnidadOperativa> uos = null;
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    uos = new List<UnidadOperativa>();
                    while (reader.Read())
                        uos.Add(GetUnidadOperativaFromReader(reader));
                }                
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

        public UnidadOperativa ObtenerUnidadOperativa(int idUnidadOperativa)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_UNIDAD_OPERATIVA_V2", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_UNIDAD_OPERATIVA", idUnidadOperativa);
            SqlDataReader reader = null;
            UnidadOperativa uos = null;
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    uos = new UnidadOperativa();
                    while (reader.Read())
                        uos = GetUnidadOperativaFromReader(reader);
                }
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

        public int InsertarUnidadOperativa(UnidadOperativa unidadOperativa)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_UNIDAD_OPERATIVA_V2", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_UNIDAD_OPERATIVA", 0).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_UNIDAD_OPERATIVA_NOMBRE", unidadOperativa.unidadOperativa);
            cmd.Parameters.AddWithValue("@PP_DESCRIPCTION", unidadOperativa.descripcion);
            cmd.Parameters.AddWithValue("@PP_IDENTIFICADOR", unidadOperativa.identificador);
            cmd.Parameters.AddWithValue("@PP_TELEFONO", unidadOperativa.telefono);
            cmd.Parameters.AddWithValue("@PP_CALLE", unidadOperativa.calle);
            cmd.Parameters.AddWithValue("@PP_NUMERO_EXTERIOR", unidadOperativa.numeroExterior);
            cmd.Parameters.AddWithValue("@PP_NUMERO_INTERIOR", unidadOperativa.numeroInterior);
            cmd.Parameters.AddWithValue("@PP_COLONIA", unidadOperativa.colonia);
            cmd.Parameters.AddWithValue("@PP_POBLACION", unidadOperativa.poblacion);
            cmd.Parameters.AddWithValue("@PP_CODIGO_POSTAL", unidadOperativa.codigoPostal);
            cmd.Parameters.AddWithValue("@PP_MUNICIPIO", unidadOperativa.municipio);
            cmd.Parameters.AddWithValue("@PP_K_ZONA", unidadOperativa.zona.idZona);
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", unidadOperativa.razonSocial.idRazonSocial);
            cmd.Parameters.AddWithValue("@PP_K_SERVIDOR", 1);
            int resultado;
            try
            {
                resultado = cmd.ExecuteNonQuery();
                if (resultado > 0)
                    unidadOperativa.idUnidadOperativa = int.Parse(cmd.Parameters["@PP_K_UNIDAD_OPERATIVA"].Value.ToString());
                return resultado;
            }
            catch (Exception e)
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

        public int ActualizarUnidadOperativa(UnidadOperativa unidadOperativa)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_UNIDAD_OPERATIVA_V2", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_UNIDAD_OPERATIVA", unidadOperativa.idUnidadOperativa);
            cmd.Parameters.AddWithValue("@PP_UNIDAD_OPERATIVA_NOMBRE", unidadOperativa.unidadOperativa);
            cmd.Parameters.AddWithValue("@PP_DESCRIPCTION", unidadOperativa.descripcion);
            cmd.Parameters.AddWithValue("@PP_IDENTIFICADOR", unidadOperativa.identificador);
            cmd.Parameters.AddWithValue("@PP_TELEFONO", unidadOperativa.telefono);
            cmd.Parameters.AddWithValue("@PP_CALLE", unidadOperativa.calle);
            cmd.Parameters.AddWithValue("@PP_NUMERO_EXTERIOR", unidadOperativa.numeroExterior);
            cmd.Parameters.AddWithValue("@PP_NUMERO_INTERIOR", unidadOperativa.numeroInterior);
            cmd.Parameters.AddWithValue("@PP_COLONIA", unidadOperativa.colonia);
            cmd.Parameters.AddWithValue("@PP_POBLACION", unidadOperativa.poblacion);
            cmd.Parameters.AddWithValue("@PP_CODIGO_POSTAL", unidadOperativa.codigoPostal);
            cmd.Parameters.AddWithValue("@PP_MUNICIPIO", unidadOperativa.municipio);
            cmd.Parameters.AddWithValue("@PP_K_ZONA", unidadOperativa.zona.idZona);
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", unidadOperativa.razonSocial.idRazonSocial);
            cmd.Parameters.AddWithValue("@PP_K_SERVIDOR", unidadOperativa.servidor.idServidor);
            int resultado;
            try
            {
                resultado = cmd.ExecuteNonQuery();
                return resultado;
            }
            catch (Exception e)
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

        public int EliminarUnidadOperativa(int idUnidadOperativa)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_DL_UNIDAD_OPERATIVA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_UNIDAD_OPERATIVA", idUnidadOperativa);
            int resultado;
            try
            {
                resultado = cmd.ExecuteNonQuery();
                return resultado;
            }
            catch (Exception e)
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

        public int ActivarUnidadOperativa(int idUnidadOperativa)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_AC_UNIDAD_OPERATIVA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_UNIDAD_OPERATIVA", idUnidadOperativa);
            int resultado;
            try
            {
                resultado = cmd.ExecuteNonQuery();
                return resultado;
            }
            catch (Exception e)
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
            uo.razonSocial.eliminado = rdr.GetBoolean(15);
            uo.servidor = new Servidor();
            uo.servidor.idServidor = rdr.GetInt32(16);
            uo.servidor.servidor = rdr.GetString(17);
            uo.zona = new Zona();
            uo.zona.idZona = rdr.GetInt32(18);
            uo.zona.zona = rdr.GetString(19);
            return uo;
        }
    }
}
