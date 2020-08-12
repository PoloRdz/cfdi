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

        public List<RegimenFiscal> getRegimenesFiscalesLista()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_REGIMENES_FISCALES_LISTA", cnn);
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

        public List<RegimenFiscal> ObtenerRegimenesFiscales(int pagina, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_REGIMENES_FISCALES", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_NUM_PAGINA", pagina);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader rdr = null;
            List<RegimenFiscal> regimenesFiscales = null;
            try
            {
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    regimenesFiscales = new List<RegimenFiscal>();
                    while (rdr.Read())
                        regimenesFiscales.Add(GetRegimenFiscalFromReader(rdr));
                }
                return regimenesFiscales;
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

        public RegimenFiscal ObtenerRegimenFiscal(int idRegimenFiscal)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_REGIMEN_FISCAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_REGIMEN_FISCAL", idRegimenFiscal);
            RegimenFiscal regimenFiscal = null;
            SqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    regimenFiscal = GetRegimenFiscalFromReader(rdr);
                }
                return regimenFiscal;
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

        public int ObtenerRegimenesFiscalesTotal()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_REGIMENES_FISCALES_TOTAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
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

        public int InsertarRegimenFiscal(RegimenFiscal regimenFiscal)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_REGIMEN_FISCAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_REGIMEN_FISCAL", regimenFiscal.idRegimenFiscal);
            cmd.Parameters.AddWithValue("@PP_DESCRIPCRION", regimenFiscal.descripcion);
            cmd.Parameters.AddWithValue("@PP_FISICA", regimenFiscal.personaFisica);
            cmd.Parameters.AddWithValue("@PP_MORAL", regimenFiscal.personaMoral);
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

        public int ActualizarRegimenFiscal(RegimenFiscal regimenFiscal)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_REGIMEN_FISCAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_REGIMEN_FISCAL", regimenFiscal.idRegimenFiscal);
            cmd.Parameters.AddWithValue("@PP_DESCRIPCRION", regimenFiscal.descripcion);
            cmd.Parameters.AddWithValue("@PP_FISICA", regimenFiscal.personaFisica);
            cmd.Parameters.AddWithValue("@PP_MORAL", regimenFiscal.personaMoral);
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

        public int EliminarRegimenFiscal(int idRegimenFiscal)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_DL_REGIMEN_FISCAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_REGIMEN_FISCAL", idRegimenFiscal);
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

        public int ActivarRegimenFiscal(int idRegimenFiscal)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_AC_REGIMEN_FISCAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_REGIMEN_FISCAL", idRegimenFiscal);
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

        private RegimenFiscal GetRegimenFiscalFromReader(SqlDataReader rdr)
        {
            var rf = new RegimenFiscal();
            rf.idRegimenFiscal = rdr.GetInt32(0);
            rf.descripcion = rdr.GetString(1);
            rf.personaFisica = rdr.GetBoolean(2);
            rf.personaMoral = rdr.GetBoolean(3);
            rf.eliminado = rdr.GetBoolean(4);
            return rf;
        }
    }
}
