﻿using CrystalDecisions.CrystalReports.Engine;
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
using WStimbrado;

namespace cfdi.Services
{
    public class TimbradoService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void Timbrar(CFDi cfdi)
        {
            if(cfdi.emisor.serie != null && cfdi.emisor.serie != "" && cfdi.folio > 0 && cfdi.idFolio > 0)
            {
                validateCFDI(cfdi);
            }
            else
            {
                createCFDI(cfdi);
            }
        }

        private void validateCFDI(CFDi cfdi)
        {
            CFDiDAO cfdiDAO = new CFDiDAO();
            cfdi.xml = cfdiDAO.getCFDIXml(cfdi.idFolio);
            if (cfdi.xml != null && cfdi.xml != "")
            {
                CfdiXmlBuilder xmlBuilder = new CfdiXmlBuilder();
                xmlBuilder.obtenerDatosTimbre(cfdi);
                if (cfdi.folioFiscal != null && cfdi.folioFiscal.Length == 36)
                {
                    sendMail(cfdi);
                }
                else
                {
                    createCFDI(cfdi);
                }
            }
        }

        private void createCFDI(CFDi cfdi)
        {
            logger.Info("Proceso de timbrado iniciado. Emisor: " + cfdi.emisor.rfcSucursal + "; Receptor: " + cfdi.receptor.rfcReceptor);
            EmisorDAO emisorDAO = new EmisorDAO();
            CFDiDAO cfdiDAO = new CFDiDAO();
            if(cfdi.pagos != null && cfdi.pagos.doctoRelacionados != null && cfdi.pagos.doctoRelacionados.Length > 0)
            {

            }
            cfdi.emisor = emisorDAO.GetIssuerInfo(cfdi.emisor.rfcSucursal, cfdi.tipoCompra.Substring(0, 1));
            cfdi.emisor.certificado = emisorDAO.GetIssuerCertInfo(cfdi.emisor.rfcSucursal);
            cfdi.importeLetra = ConvertidorImporte.enletras(cfdi.total, cfdi.moneda);
            cfdi.fechaCert = DateTime.Now;
            cfdiDAO.saveCFDI(cfdi, true);
            if (cfdi.folio > 0)
            {
                CfdiXmlBuilder xmlBuilder = new CfdiXmlBuilder();
                cfdi.xml = xmlBuilder.BuildXml(cfdi);
                var timbreRespuesta = new respuestaTimbrado();
                cfdiDAO.saveCFDI(cfdi, false);
                //timbrar xml
                //timbrarFacturaWS(cfdi);
                //Obtener los datos del xml timbrado
                //xmlBuilder.obtenerDatosTimbre(cfdi);
                cfdiDAO.saveCFDI(cfdi, false);
                logger.Info("Cadena original del complemento de certificacion digital del SAT: " + cfdi.cadenaCertificadoSat);
                sendMail(cfdi);
            }
            else
            {
                throw new InvalidCfdiDataException("No fue posible guardar los datos de la factura");
            }
        }

        private void sendMail(CFDi cfdi)
        {
            Stream xmlStream = StreamBuilder.getStreamFromString(cfdi.xml);
            cfdi.xml = null;
            cfdi.emisor.certificado = null;
            Thread mailingThread = new Thread(
                delegate ()
                {
                    MailSender.sendMail("Factura electrónica", new string[1] { cfdi.receptor.email }, xmlStream /*, agregarPdf*/);
                }
            );
            mailingThread.Start();
        }

        public CFDi cancelarTimbre(CFDi cfdi)
        {
            logger.Info("Proceso de cancelación de factura iniciado. Serie: " + cfdi.serie + "; Folio: " + cfdi.folio);
            CFDiDAO cfdiDAO = new CFDiDAO();
            if (!cfdiDAO.validateInvoiceStatus(cfdi.serie, cfdi.folio))
                throw new InvalidInvoiceStatusException("El estatus actual de la factura no permite cancelar");
            cfdi = cfdiDAO.getInvoiceInfo(cfdi.serie, cfdi.folio);
            CfdiXmlBuilder builder = new CfdiXmlBuilder();
            cfdi.xml = builder.BuildCancelacionXml(cfdi);
            //WS cancelar
            timbrarFacturaWS(cfdi);
            cfdiDAO.cancelarTimbre(cfdi);
            return cfdi;
        }

        public void timbrarFacturaWS(CFDi cfdi)
        {
            string xml = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cfdi.xml));
            string user = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("UserGTomzaWS"));
            string password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("WSTomza20."));
            try
            {
                generaCFDIPortType generaCFDiService = new generaCFDIPortTypeClient();
                respuestaTimbrado respuestaTimbre = new respuestaTimbrado();

                respuestaTimbre = generaCFDiService.generaCFDIAsync(user, password, xml).GetAwaiter().GetResult();

                validateResponse(respuestaTimbre);

                cfdi.xml = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(respuestaTimbre.documentoProcesado));
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw new WebServiceCommunicationException("Error desconocido de comunicación");
            }
        }

        private void validateResponse(respuestaTimbrado respuesta)
        {
            logger.Info(respuesta.codigoResultado + " : " + respuesta.codigoDescripcion);
            if(Operators.CompareString(respuesta.codigoResultado, null, false) == 0)
            {
                logger.Error("Error de conexión con el servicio de timbrado: " + respuesta.codigoResultado);
                throw new WebServiceCommunicationException("Error de concexión con el servicio de timbrado");
            }
            else if(Operators.CompareString(respuesta.codigoResultado, "100", false) != 0 
                    && Operators.CompareString(respuesta.codigoResultado, "201", false) != 0 
                    && Operators.CompareString(respuesta.codigoResultado, "200", false) != 0)
            {
                logger.Error("Error de timbrado: " + respuesta.codigoResultado + " : " + respuesta.codigoDescripcion);
                throw new WebServiceValidationException(respuesta.codigoDescripcion);
            }
            else if(Operators.CompareString(respuesta.codigoDescripcion, "El resultado de la digesti&oacute;n debe ser igual al resultado de la desencripci&oacute;n del sello.", false) == 0)
            {
                logger.Error("Error de timbrado: " + respuesta.codigoResultado + " : " + respuesta.codigoDescripcion);
                throw new WebServiceValidationException(respuesta.codigoDescripcion);
            }
        }
    }
}
