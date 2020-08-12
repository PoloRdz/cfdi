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
        public List<UsoCFDi> getUsoCFDisLista()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USO_CFDIS_LISTA", cnn);
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

        public List<UsoCFDi> ObtenerUsoCFDis(int pagina, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USO_CFDIS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_NUM_PAGINA", pagina);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader rdr = null;
            List<UsoCFDi> usoCFDis = null;
            try
            {
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    usoCFDis = new List<UsoCFDi>();
                    while (rdr.Read())
                        usoCFDis.Add(getUsoCFDiFromReader(rdr));
                }
                return usoCFDis;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                rdr.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public int ObtenerUsoCFDisTotal()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USO_CFDIS_TOTAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            int total;
            SqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                rdr.Read();
                total = rdr.GetInt32(0);
                return total;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                rdr.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public UsoCFDi ObtenerUsoCFDi(string usoCFDi)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_USO_CFDI", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_USO_CFDI", usoCFDi);
            SqlDataReader rdr = null;
            UsoCFDi usoCFDI = null;
            try
            {
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    usoCFDI = new UsoCFDi();
                    rdr.Read();
                    usoCFDI = getUsoCFDiFromReader(rdr);
                }
                return usoCFDI;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                rdr.Close();
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public int InsertarUsoCFDis(UsoCFDi usoCFDi)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_USO_CFDI", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_USO_CFDI", usoCFDi.usoCFDi);
            cmd.Parameters.AddWithValue("@PP_DESCRIPCRION", usoCFDi.descripcion);
            cmd.Parameters.AddWithValue("@PP_EXPLICACION", usoCFDi.explicacion);
            cmd.Parameters.AddWithValue("@PP_FISICA", usoCFDi.fisica);
            cmd.Parameters.AddWithValue("@PP_MORAL", usoCFDi.moral);
            int total;
            try
            {
                total = cmd.ExecuteNonQuery();
                return total;
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

        public int ActualizarUsoCFDis(UsoCFDi usoCFDi)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_USO_CFDI", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_USO_CFDI", usoCFDi.usoCFDi);
            cmd.Parameters.AddWithValue("@PP_DESCRIPCRION", usoCFDi.descripcion);
            cmd.Parameters.AddWithValue("@PP_EXPLICACION", usoCFDi.explicacion);
            cmd.Parameters.AddWithValue("@PP_FISICA", usoCFDi.fisica);
            cmd.Parameters.AddWithValue("@PP_MORAL", usoCFDi.moral);
            int total;
            try
            {
                total = cmd.ExecuteNonQuery();
                return total;
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

        public int EliminarUsoCFDis(string usoCFDi)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_DL_USO_CFDI", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_USO_CFDI", usoCFDi);
            int total;
            try
            {
                total = cmd.ExecuteNonQuery();
                return total;
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

        public int ActivarUsoCFDis(string usoCFDi)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_AC_USO_CFDI", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_USO_CFDI", usoCFDi);
            int total;
            try
            {
                total = cmd.ExecuteNonQuery();
                return total;
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

        private UsoCFDi getUsoCFDiFromReader(SqlDataReader rdr)
        {
            UsoCFDi uso = new UsoCFDi
            {
                usoCFDi = rdr.GetString(0),
                descripcion = rdr.GetString(1),
                explicacion = rdr.GetString(2),
                fisica = rdr.GetBoolean(3),
                moral = rdr.GetBoolean(4),
                eliminado = rdr.GetBoolean(5)
            };
            return uso;
        }
    }
}
