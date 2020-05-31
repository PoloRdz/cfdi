using cfdi.Exceptions;
using cfdi.models;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.HPack;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Data.DAO
{
    public class EmisorDAO
    {
        public Certificado GetIssuerCertInfo(string rfc)
        {
            Certificado cert = new Certificado();
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand command;
            SqlDataReader reader;
            string script = "SELECT C.CERTIFICADO, C.LLAVE, C.CONTRASENA, RUTA_CERTIFICADO FROM CERTIFICADO C INNER JOIN FISCALES_EMISOR FE ON FE.K_CERTIFICADO = C.K_CERTIFICADO INNER JOIN RAZON_SOCIAL RS ON RS.K_RAZON_SOCIAL = FE.K_RAZON_SOCIAL WHERE RS.RFC_RAZON_SOCIAL = '" + rfc + "'";
            command = new SqlCommand(script, cnn);
            reader = command.ExecuteReader();
            cnn.Close();
            if (!reader.HasRows)
            {
                throw new InvalidRFCException("RFC proporcionado no es válido o no existe");
            }
            reader.Read();
            cert.cert = reader.GetValue(0).ToString();
            cert.key = reader.GetValue(1).ToString();
            cert.contrasena = reader.GetValue(2).ToString();
            cert.rutaCert = reader.GetValue(3).ToString();
            return cert;
        }

        public Emisor GetIssuerInfo(string rfc) 
        {
            Emisor emisor = new Emisor();
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand command;
            SqlDataReader reader;
            string script = "Script para obtener datos del emisor";
            command = new SqlCommand(script, cnn);
            reader = command.ExecuteReader();
            cnn.Close();
            if (!reader.HasRows)
            {
                throw new InvalidRFCException("RFC proporcionado no es válido o no existe");
            }
            reader.Read();
            emisor.idSucursal = int.Parse(reader.GetValue(0).ToString());
            emisor.rfcSucursal = reader.GetValue(1).ToString();
            emisor.sucursal = reader.GetValue(2).ToString();
            return emisor;
        }
    }
}
