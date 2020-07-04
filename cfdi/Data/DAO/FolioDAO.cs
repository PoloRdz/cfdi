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
    public class FolioDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public int getFoliosCount()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_FOLIOS_COUNT", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_TOTAL_REG", 0).Direction = ParameterDirection.InputOutput;
            int total = 0;
            try
            {
                cmd.ExecuteNonQuery();
                total = (int)cmd.Parameters["@PP_TOTAL_REG"].Value;
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
                cnn.Close();
            }
        }

        public List<Folio> getFolios(int page, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_FOLIOS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_NUM_PAGINA", page);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader reader = cmd.ExecuteReader();
            var series = new List<Folio>();
            try
            {
                if (!reader.HasRows)
                    throw new NotFoundException("No se han encontrado folios");
                while (reader.Read())
                {
                    series.Add(getFolioFromReader(reader));
                }
                return series;
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
                cnn.Close();
            }
        }

        public Folio getFolio(int id)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_FOLIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_FOLIO", id);
            Folio folio = null;
            SqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("No se ha encontrado el folio con id: " + id);
                reader.Read();
                folio = getFolioFromReader(reader);
                return folio;
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

        public Folio insertFolio(Folio folio)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_FOLIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_FOLIO", 0).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", folio.emisor.unidadOperativa.razonSocial.idRazonSocial);
            cmd.Parameters.AddWithValue("@PP_FOLIOS", folio.folios);
            try
            {
                cmd.ExecuteNonQuery();
                int id = (int)cmd.Parameters["@PP_ID_FOLIO"].Value;
                if (id <= 0)
                    throw new Exception("Error al insertar el folio");
                folio.id = id;
                return folio;

            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public void updateFolio(Folio folio)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_FOLIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_FOLIO", folio.id);
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", folio.emisor.unidadOperativa.razonSocial.idRazonSocial);
            cmd.Parameters.AddWithValue("@PP_FOLIOS", folio.folios);
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows == 0)
                    throw new Exception("La serie no ha sido actualizada");
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public void deleteFolio(int id)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_DL_FOLIO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_FOLIO", id);
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows == 0)
                    throw new Exception("El folio no ha sido eliminado");
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        private Folio getFolioFromReader(SqlDataReader rdr)
        {
            Folio folio = new Folio();
            folio.id = rdr.GetInt32(0);
            folio.folios = rdr.GetInt32(1);
            folio.emisor = new Emisor 
            {
                unidadOperativa = new UnidadOperativa
                {
                    razonSocial = new RazonSocial
                    {
                        idRazonSocial = rdr.GetInt32(2),
                        razonSocial = rdr.GetValue(3).ToString(),
                        rfc = rdr.GetValue(4).ToString()
                    }
                }
            };
            return folio;
        }
    }
}
