using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Data;
using cfdi.Models;

namespace cfdi.Services
{
    public class TimbradoService
    {
        public void Timbrar(CFDi cfdi)
        {
            GetIssuerCertInfo(cfdi);
            XmlBuilderService xmlBuilder = new XmlBuilderService();
            cfdi.xml = xmlBuilder.BuildXml(cfdi);
        }

        public void GetIssuerCertInfo(CFDi cfdi)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand command;
            SqlDataReader reader;
            string script = "SELECT C.CERTIFICADO, C.LLAVE, C.CONTRASENA, RUTA_CERTIFICADO FROM CERTIFICADO C INNER JOIN FISCALES_EMISOR FE ON FE.K_CERTIFICADO = C.K_CERTIFICADO INNER JOIN RAZON_SOCIAL RS ON RS.K_RAZON_SOCIAL = FE.K_RAZON_SOCIAL WHERE RS.RFC_RAZON_SOCIAL = '" + cfdi.rfcSucursal + "'";
            command = new SqlCommand(script, cnn);
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                cfdi.cert = reader.GetValue(0).ToString();
                cfdi.key = reader.GetValue(1).ToString();
                cfdi.contrasena = reader.GetValue(2).ToString();
                cfdi.rutaCert = reader.GetValue(3).ToString();
            }
            else
            {
                throw new Exception("RFC proporcionado no es válido o no existe");
            }
        }
    }
}
