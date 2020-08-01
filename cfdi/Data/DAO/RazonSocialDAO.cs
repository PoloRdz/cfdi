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
    public class RazonSocialDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<RazonSocial> getRazonesSociales(int pagina, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_RAZONES_SOCIALES", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_NUM_PAGINA", pagina);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader rdr = null;
            List<RazonSocial> rss = new List<RazonSocial>();
            try
            {
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new NotFoundException("No se han encontrado Razones Sociales");
                while (rdr.Read())
                    rss.Add(getRazonSocialFromReader(rdr));
                return rss;
            }
            catch(Exception e)
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

        public List<RazonSocial> getRazonesSociales()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_RAZONES_SOCIALES_TODAS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = null;
            List<RazonSocial> rss = new List<RazonSocial>();
            try
            {
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new NotFoundException("No se han encontrado Razones Sociales");
                while (rdr.Read())
                    rss.Add(getRazonSocialFromReader(rdr));
                return rss;
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

        public int getRazonesSocialesTotal()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_RAZONES_SOCIALES_TOTAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new NotFoundException("No se han encontrado Razones Sociales");
                rdr.Read();
                return rdr.GetInt32(0);
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

        public RazonSocial getRazonSocial(int idRazonSocial)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_RAZON_SOCIAL_V2", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", idRazonSocial);
            SqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new NotFoundException("No se ha encontrado la Razón Social");
                rdr.Read();
                return getRazonSocialFromReader(rdr);
            }
            catch(Exception e)
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

        public int InsertRazonSocial(RazonSocial rs)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_RAZON_SOCIAL_V2", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", 0).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_RAZON_SOCIAL", rs.razonSocial);
            cmd.Parameters.AddWithValue("@PP_NOMBRE_CORTO", rs.nombreCorto);
            cmd.Parameters.AddWithValue("@PP_IDENTIFICADOR", rs.identificador);
            cmd.Parameters.AddWithValue("@PP_RFC", rs.rfc);
            cmd.Parameters.AddWithValue("@PP_CURP", rs.curp);
            cmd.Parameters.AddWithValue("@PP_CORREO", rs.correo);
            cmd.Parameters.AddWithValue("@PP_TELEFONO", rs.correo);
            cmd.Parameters.AddWithValue("@PP_CALLE", rs.calle);
            cmd.Parameters.AddWithValue("@PP_NUMERO_EXTERIOR", rs.numeroExterior);
            cmd.Parameters.AddWithValue("@PP_NUMERO_INTERIOR", rs.numeroInterior);
            cmd.Parameters.AddWithValue("@PP_COLONIA", rs.colonia);
            cmd.Parameters.AddWithValue("@PP_CODIGO_POSTAL", rs.codigoPostal);
            cmd.Parameters.AddWithValue("@PP_MUNICIPIO", rs.municipio);
            try
            {
                cmd.ExecuteNonQuery();
                int id = (int) cmd.Parameters["@PP_K_RAZON_SOCIAL"].Value;
                if (id == 0)
                    throw new Exception("No fue posible guardar la razón social");
                return id;
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

        public bool UpdateRazonSocial(RazonSocial rs)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_RAZON_SOCIAL_V2", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", rs.idRazonSocial);
            cmd.Parameters.AddWithValue("@PP_RAZON_SOCIAL", rs.razonSocial);
            cmd.Parameters.AddWithValue("@PP_NOMBRE_CORTO", rs.nombreCorto);
            cmd.Parameters.AddWithValue("@PP_IDENTIFICADOR", rs.identificador);
            cmd.Parameters.AddWithValue("@PP_RFC", rs.rfc);
            cmd.Parameters.AddWithValue("@PP_CURP", rs.curp);
            cmd.Parameters.AddWithValue("@PP_CORREO", rs.correo);
            cmd.Parameters.AddWithValue("@PP_TELEFONO", rs.correo);
            cmd.Parameters.AddWithValue("@PP_CALLE", rs.calle);
            cmd.Parameters.AddWithValue("@PP_NUMERO_EXTERIOR", rs.numeroExterior);
            cmd.Parameters.AddWithValue("@PP_NUMERO_INTERIOR", rs.numeroInterior);
            cmd.Parameters.AddWithValue("@PP_COLONIA", rs.colonia);
            cmd.Parameters.AddWithValue("@PP_CODIGO_POSTAL", rs.codigoPostal);
            cmd.Parameters.AddWithValue("@PP_MUNICIPIO", rs.municipio);
            try
            {
                int id =  cmd.ExecuteNonQuery();
                if (id == 0)
                    throw new Exception("No fue posible actualizar la razón social");
                return true;
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

        public bool DeleteRazonSocial(int idRazonSocial)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_DL_RAZON_SOCIAL_V2", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", idRazonSocial);
            try
            {
                int id = cmd.ExecuteNonQuery();
                if (id == 0)
                    throw new Exception("No fue posible dar de baja la razón social");
                return true;
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

        private RazonSocial getRazonSocialFromReader(SqlDataReader rdr)
        {
            var rs = new RazonSocial
            {
                idRazonSocial = rdr.GetInt32(0),
                razonSocial = rdr.GetString(1),
                nombreCorto = rdr.GetString(2),
                identificador = rdr.GetString(3),
                rfc = rdr.GetString(4),
                curp = rdr.GetString(5),
                correo = rdr.GetString(6),
                telefono = rdr.GetString(7),
                calle = rdr.GetString(8),
                numeroExterior = rdr.GetString(9),
                numeroInterior = rdr.GetString(10),
                colonia = rdr.GetString(11),
                codigoPostal = rdr.GetString(12),
                municipio = rdr.GetString(13)
            };
            return rs;
        }
    }
}
