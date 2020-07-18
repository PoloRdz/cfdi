using cfdi.Exceptions;
using cfdi.Models;
using iTextSharp.text.xml.simpleparser.handler;
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

        private Certificado getCertificadoFromReader(SqlDataReader rdr)
        {
            Certificado cert = new Certificado
            {
                idCertificado = rdr.GetInt32(0),
                cert = rdr.GetString(1),
                rutaCert = rdr.GetString(2),
                key = rdr.GetString(3),
                contrasena = rdr.GetString(4),
                fechaExpiracion = rdr.GetDateTime(5)
            };
            return cert;
        }
    }
}
