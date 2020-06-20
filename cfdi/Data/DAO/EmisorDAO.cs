using cfdi.Exceptions;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.HPack;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Models.DTO;
using cfdi.Models;

namespace cfdi.Data.DAO
{
    public class EmisorDAO
    {
        public Certificado GetIssuerCertInfo(string rfc)
        {
            Certificado cert = new Certificado();
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand command = new SqlCommand("PG_SK_CERT_INFO_EMISOR", cnn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            command.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            command.Parameters.AddWithValue("@PP_RFC_EMISOR", rfc);
            SqlDataReader reader = command.ExecuteReader(); 
            if (!reader.HasRows)
            {
                throw new InvalidRFCException("RFC proporcionado no es válido o no existe");
            }
            reader.Read();
            cert.cert = reader.GetValue(0).ToString();
            cert.key = reader.GetValue(1).ToString();
            cert.contrasena = reader.GetValue(2).ToString();
            cert.rutaCert = reader.GetValue(3).ToString();
            reader.Close();
            cnn.Close();
            return cert;
        }

        public Emisor GetIssuerInfo(string rfc, string tipoCompra)
        {
            Emisor emisor = new Emisor();
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand command = new SqlCommand("PG_SK_INFO_EMISOR", cnn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            command.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            command.Parameters.AddWithValue("@PP_RFC_EMISOR", rfc);
            command.Parameters.AddWithValue("@PP_TIPO_COMPRA", tipoCompra);
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                throw new InvalidRFCException("RFC proporcionado no es válido o no existe");
            }
            reader.Read();
            emisor.idSucursal = int.Parse(reader.GetValue(0).ToString());
            emisor.rfcSucursal = reader.GetValue(1).ToString();
            emisor.sucursal = reader.GetValue(2).ToString();
            emisor.regimenFiscal = reader.GetValue(3).ToString();
            emisor.codigoPostal = reader.GetValue(4).ToString();
            emisor.serie = reader.GetValue(5).ToString();
            cnn.Close();
            return emisor;
        }
    }
}
