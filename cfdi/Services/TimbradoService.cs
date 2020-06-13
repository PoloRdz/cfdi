using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using cfdi.Data;
using cfdi.Data.DAO;
using System.Web;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Utils;
using Microsoft.VisualBasic.CompilerServices;
using ServiceReference1;

namespace cfdi.Services
{
    public class TimbradoService
    {
        public void Timbrar(CFDi cfdi)
        {
            EmisorDAO emisorDAO = new EmisorDAO();
            CFDiDAO cfdiDAO = new CFDiDAO();
            cfdi.emisor = emisorDAO.GetIssuerInfo(cfdi.emisor.rfcSucursal);
            cfdi.emisor.certificado = emisorDAO.GetIssuerCertInfo(cfdi.emisor.rfcSucursal);
            cfdi.importeLetra = ConvertidorImporte.enletras(cfdi.total, cfdi.moneda);
            cfdi.fechaCert = DateTime.Now;
            cfdiDAO.saveCFDI(cfdi, true);
            if(cfdi.folio > 0) 
            {
                CfdiXmlBuilder xmlBuilder = new CfdiXmlBuilder();
                cfdi.xml = xmlBuilder.BuildXml(cfdi);
                //timbrar xml
                //timbrarFacturaWS(cfdi);
                ////Obtener los datos del xml timbrado
                //xmlBuilder.obtenerDatosTimbre(cfdi);
                cfdiDAO.saveCFDI(cfdi, false);

                Stream xmlStream = StreamBuilder.getStreamFromString(cfdi.xml);
                //cfdi.xml = null;
                cfdi.emisor.certificado = null;
                Thread mailSendThread = new Thread(
                    delegate ()
                    {
                        MailSender.sendMail("Factura electrónica", new string[1] { cfdi.receptor.email }, xmlStream /*, agregarPdf*/);
                    }
                );
                mailSendThread.Start();
            }
            else
            {
                throw new InvalidCfdiDataException("No fue posible guardar los datos de la factura");
            }
        }

        public CFDi cancelarTimbre(CFDi cfdi)
        {
            CFDiDAO cfdiDAO = new CFDiDAO();
            if (!cfdiDAO.validateInvoiceStatus(cfdi.serie, cfdi.folio))
                throw new InvalidInvoiceStatusException("El estatus actual de la factura no permite cancelar");
            cfdi = cfdiDAO.getInvoiceInfo(cfdi.serie, cfdi.folio);
            CfdiXmlBuilder builder = new CfdiXmlBuilder();
            cfdi.xml = builder.BuildCancelacionXml(cfdi);
            //WS cancelar
            cfdiDAO.cancelarTimbre(cfdi);
            return cfdi;
        }

        public async void timbrarFacturaWS(CFDi cfdi)
        {
            string xml = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cfdi.xml));
            string user = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("UserGTomzaWS"));
            string password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("WSTomza20."));

            generaCFDIPortType generaCFDiService = new generaCFDIPortTypeClient();
            respuestaTimbrado respuestaTimbre = new respuestaTimbrado();

            respuestaTimbre = await generaCFDiService.generaCFDIAsync(user, password, xml);

            validateResponse(respuestaTimbre); 

            cfdi.xml = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(respuestaTimbre.documentoProcesado));
        }

        private void validateResponse(respuestaTimbrado respuesta)
        {
            if(Operators.CompareString(respuesta.codigoResultado, null, false) == 0)
            {
                throw new Exception("Error de concexión con el servicio de timbrado");
            }
            if(Operators.CompareString(respuesta.codigoResultado, "100", false) != 0 && Operators.CompareString(respuesta.codigoResultado, "201", false) != 0)
            {
                throw new Exception(respuesta.codigoDescripcion);
            }
            if(Operators.CompareString(respuesta.codigoDescripcion, "El resultado de la digesti&oacute;n debe ser igual al resultado de la desencripci&oacute;n del sello.", false) == 0)
            {
                throw new Exception(respuesta.codigoDescripcion);
            }
        }
    }
}
