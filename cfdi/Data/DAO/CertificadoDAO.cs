using cfdi.Exceptions;
using cfdi.Models;
using iTextSharp.text.io;
using iTextSharp.text.xml.simpleparser.handler;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace cfdi.Data.DAO
{
    public class CertificadoDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<Certificado> GetCertificados(int pagina, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_CERTIFICADOS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_NUM_PAGINA", pagina);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader rdr = null;
            var certs = new List<Certificado>();
            try
            {
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new NotFoundException("No se han encontrado Certificados");
                while (rdr.Read())
                    certs.Add(getCertificadoFromReader(rdr));
                return certs;
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

        public int getCertificadosTotal()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_CERTIFICADOS_TOTAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
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

        public Certificado GetCertificado(int idCertificado)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_CERTIFICADO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_K_CERTIFICADO", idCertificado);
            SqlDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new NotFoundException("No se ha encontrado el certificado");
                rdr.Read();
                return getCertificadoFromReader(rdr);
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

        public int InsertCertificado(Certificado cert, int idRazonSocial)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_IN_CERTIFICADO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_CERTIFICADO", 0).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", idRazonSocial);
            cmd.Parameters.AddWithValue("@PP_D_CERTIFICADO", cert.descripcion);
            cmd.Parameters.AddWithValue("@PP_C_CERTIFICADO", cert.identificador);
            cmd.Parameters.AddWithValue("@PP_CERTIFICADO", cert.cert + ".cer");
            cmd.Parameters.AddWithValue("@PP_RUTA_CERTIFICADO", "C:/Tomza.SYS/CERTIFICADOS_ERP/" + cert.rutaCert);
            cmd.Parameters.AddWithValue("@PP_LLAVE", cert.key + ".key");
            cmd.Parameters.AddWithValue("@PP_CONTRASENA", cert.contrasena);
            cmd.Parameters.AddWithValue("@PP_LOGO", "logo_" + cert.cert + ".jpg");
            cmd.Parameters.AddWithValue("@PP_FECHA_EXPIRACION", "1900-01-01 00:00:00.000");
            try
            {
                cmd.ExecuteNonQuery();
                int id = (int)cmd.Parameters["@PP_ID_CERTIFICADO"].Value;
                if (id == 0)
                    throw new Exception("No fue posible guardar el certificado");
                return id;
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

        public bool UpdateCertificado(Certificado cert)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_CERTIFICADO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_CERTIFICADO", cert.idCertificado);
            cmd.Parameters.AddWithValue("@PP_D_CERTIFICADO", cert.descripcion);
            cmd.Parameters.AddWithValue("@PP_C_CERTIFICADO", cert.identificador);
            cmd.Parameters.AddWithValue("@PP_CERTIFICADO", cert.cert);
            cmd.Parameters.AddWithValue("@PP_RUTA_CERTIFICADO", cert.rutaCert);
            cmd.Parameters.AddWithValue("@PP_LLAVE", cert.key);
            cmd.Parameters.AddWithValue("@PP_CONTRASENA", cert.contrasena);
            cmd.Parameters.AddWithValue("@PP_LOGO", "logo_" + cert.cert);
            cmd.Parameters.AddWithValue("@PP_FECHA_EXPIRACION", cert.fechaExpiracion);
            try
            {
                int id = cmd.ExecuteNonQuery();
                if (id == 0)
                    throw new Exception("No fue posible actualizar el certificado");
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

        public bool RemoveCertificado(int idCertificado)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_DL_CERTIFICADO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_CERTIFICADO", idCertificado);
            try
            {
                int id = cmd.ExecuteNonQuery();
                if (id == 0)
                    throw new Exception("No fue posible remover el certificado");
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

        public bool ActivarCertificado(int idCertificado)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_AC_CERTIFICADO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_CERTIFICADO", idCertificado);
            try
            {
                int id = cmd.ExecuteNonQuery();
                if (id == 0)
                    throw new Exception("No fue posible activar el certificado");
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

        private Certificado getCertificadoFromReader(SqlDataReader rdr)
        {
            Certificado cert = new Certificado
            {
                idCertificado = rdr.GetInt32(0),
                cert = rdr.GetString(1),
                descripcion = rdr.GetString(2),
                identificador = rdr.GetString(3),
                rutaCert = rdr.GetString(4),
                key = rdr.GetString(5),
                contrasena = rdr.GetString(6),
                fechaExpiracion = rdr.GetDateTime(7),
                eliminado = rdr.GetBoolean(8)
            };
            return cert;
        }
    }
}
