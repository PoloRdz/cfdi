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

        public List<Zona> getZonasLista()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_ZONAS_LISTA", cnn);
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

        public List<Zona> ObtenerZonas(int pagina, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_ZONAS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_PAGINA", pagina);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader reader = null;
            List<Zona> zonas = new List<Zona>();
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return null;
                while (reader.Read())
                {
                    zonas.Add(getZonaFromReader(reader));
                }
                return zonas;
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

        public int ObtenerZonasTotal()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_ZONAS_TOTAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            int zonasTotal;
            SqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                zonasTotal = reader.GetInt32(0);
                return zonasTotal;
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

        public Zona getZona(int idZona)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_ZONA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_ZONA", idZona);
            SqlDataReader reader = null;
            Zona zona = new Zona();
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return null;
                reader.Read();
                zona = getZonaFromReader(reader);
                return zona;
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

        public int InsertarZona(Zona zona)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_ZONA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_ZONA", 0).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_ZONA_NOMBRE", zona.zona);
            cmd.Parameters.AddWithValue("@PP_DESCRIPCTION", zona.descripcion);
            cmd.Parameters.AddWithValue("@PP_IDENTIFICADOR", zona.identificador);
            int resultado;
            try
            {
                resultado = cmd.ExecuteNonQuery();
                if (resultado > 0)
                    zona.idZona = int.Parse(cmd.Parameters["@PP_K_ZONA"].Value.ToString());
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

        public int ActualizarZona(Zona zona)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_ZONA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_ZONA", zona.idZona);
            cmd.Parameters.AddWithValue("@PP_ZONA_NOMBRE", zona.zona);
            cmd.Parameters.AddWithValue("@PP_DESCRIPCTION", zona.descripcion);
            cmd.Parameters.AddWithValue("@PP_IDENTIFICADOR", zona.identificador);
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

        public int EliminarZona(int idZona)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_DL_ZONA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_ZONA", idZona);
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

        public int ActivarZona(int idZona)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_AC_ZONA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_ZONA", idZona);
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

        private Zona getZonaFromReader(SqlDataReader rdr)
        {
            Zona zona = new Zona();
            zona.idZona = rdr.GetInt32(0);
            zona.zona = rdr.GetString(1);
            zona.descripcion = rdr.GetString(2);
            zona.identificador = rdr.GetString(3);
            zona.eliminado = rdr.GetBoolean(4);
            return zona;
        }
    }
}
