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
    public class SerieDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public int getSeriesCount()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_SERIES_COUNT", cnn);
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

        public List<Serie> getSeries(int page, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_SERIES", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_NUM_PAGINA", page);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader reader = cmd.ExecuteReader();
            var series = new List<Serie>();
            try
            {
                if (!reader.HasRows)
                    throw new NotFoundException("No se han encontrado series");
                while (reader.Read())
                {
                    series.Add(getSerieFromReader(reader));
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

        public Serie getSerie(int id)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_SERIE", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_SERIE", id);
            Serie serie = null;
            SqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new NotFoundException("No se ha encontrado la serie con id: " + id);
                reader.Read();
                serie = getSerieFromReader(reader);
                return serie;
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

        public Serie insertSerie(Serie serie)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_SERIE", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_SERIE", 0).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", serie.emisor.idSucursal);
            cmd.Parameters.AddWithValue("@PP_TIPO_VENTA", serie.tipoVenta);
            cmd.Parameters.AddWithValue("@PP_SERIE", serie.serie);
            try
            {
                cmd.ExecuteNonQuery();
                int id = (int)cmd.Parameters["@PP_ID_SERIE"].Value;
                if (id <= 0)
                    throw new Exception("Error al insertar la serie");
                serie.id = id;
                return serie;

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

        public void updateSerie(Serie serie)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_SERIE", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_SERIE", serie.id);
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", serie.emisor.idSucursal);
            cmd.Parameters.AddWithValue("@PP_TIPO_VENTA", serie.tipoVenta);
            cmd.Parameters.AddWithValue("@PP_SERIE", serie.serie);
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows == 0)
                    throw new Exception("La serie no ha sido actualizada");
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cnn.Dispose();
            }
        }

        public void deleteSerie(int id)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_DL_SERIE", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_SERIE", id);
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows == 0)
                    throw new Exception("La serie no ha sido eliminada");
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

        private Serie getSerieFromReader(SqlDataReader rdr)
        {
            Serie serie = new Serie();
            serie.id = rdr.GetInt32(0);
            serie.tipoVenta = rdr.GetValue(1).ToString();
            serie.serie = rdr.GetValue(2).ToString();
            serie.emisor = new Emisor();
            serie.emisor.idSucursal = rdr.GetInt32(3);
            serie.emisor.sucursal = rdr.GetValue(4).ToString();
            serie.emisor.rfcSucursal = rdr.GetValue(5).ToString();
            return serie;
        }
    }
}
