﻿using System;
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
            cfdi.mPago = "PUE";
            cfdi.moneda = "MXN";
            cfdi.tipoVenta = "DEBITO";
            cfdi.formaPago = "99";
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
            //logger.Info("Proceso de timbrado iniciado. Emisor: " + cfdi.emisor.unidadOperativa.razonSocial.rfc + "; Receptor: " + cfdi.receptor.informacionFiscal.rfc);
            EmisorDAO emisorDAO = new EmisorDAO();
            ConceptosDAO conDAO = new ConceptosDAO();
            CFDiDAO cfdiDAO = new CFDiDAO();
            UsuarioDAO uDAO = new UsuarioDAO();
            Calculador calc = new Calculador();
            //if (cfdi.pagos != null && cfdi.pagos.doctoRelacionados != null && cfdi.pagos.doctoRelacionados.Length > 0)
            //{
            //    getDoctoRelacionados(cfdi.pagos.doctoRelacionados, cfdiDAO);
            //}
            cfdi.emisor = emisorDAO.GetIssuerInfo(cfdi.emisor.unidadOperativa.idUnidadOperativa,
                                                  cfdi.tipoCompra.Substring(0, 1));
            cfdi.emisor.certificado = emisorDAO.GetIssuerCertInfo(cfdi.emisor.unidadOperativa.razonSocial.idRazonSocial);
            cfdi.receptor.usuario = uDAO.getUsuario(cfdi.receptor.usuario.id);
            cfdi.receptor.informacionFiscal = uDAO.getUsuarioFiscales(cfdi.receptor.usuario.id);
            cfdi.conceptos = conDAO.getConceptos(cfdi.idMov, cfdi.emisor.unidadOperativa.idUnidadOperativa);
            //if(cfdi.conceptos[0].fecha.ToString("yyyy/MM/dd").Equals(DateTime.Now.ToString("yyyy/MM/dd")))
            //{
            //    throw new SameDayInvoiceException("No puedes hacer la factura el mismo día de la compra");
            //}
            //if(!cfdi.conceptos[0].fecha.ToString("yyyy/MM/dd").Equals(cfdi.fecha.ToString("yyyy/MM/dd")))
            //{
            //    throw new InvoiceDateMismatchException("La fecha que has ingresado no coincide con la fecha de la compra");
            //}
            //if(DateTime.Now.Subtract(cfdi.conceptos[0].fecha).TotalDays >= 15)
            //{
            //    throw new ExpiredInvoiceException("Solo puedes facturar hasta 15 dias después de la compra");
            //}
            //Validar si los conceptos no han sido facturados
            calc.calcularDescuentosConceptos(cfdi.conceptos);
            calc.calcularImpuestoConceptos(cfdi.conceptos, cfdi.emisor.unidadOperativa);
            calc.calcularTotal(cfdi);
            cfdi.importeLetra = ConvertidorImporte.enletras(cfdi.total, cfdi.moneda);
            cfdi.fechaCert = DateTime.Now;
            cfdiDAO.saveCFDI(cfdi, true);
            if (cfdi.folio > 0)
            {
                CfdiXmlBuilder xmlBuilder = new CfdiXmlBuilder();
                cfdi.xml = xmlBuilder.BuildXml(cfdi);

                cfdiDAO.saveCFDI(cfdi, false);
                //timbrarFacturaWS(cfdi);
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

        public void getDoctoRelacionados(DoctoRelacionado[] doctoRelacionados, CFDiDAO cfdiDAO)
        {
            foreach (DoctoRelacionado docRelacionado in doctoRelacionados)
            {
                DoctoRelacionado relacion = null;
                relacion = cfdiDAO.getDoctoRelacionadoInfo(docRelacionado.folio, docRelacionado.serie);
                if (relacion != null)
                {
                    docRelacionado.idDocumento = relacion.idDocumento;
                    docRelacionado.impSaldoAnt = relacion.impSaldoAnt;
                    docRelacionado.numParcialidad = relacion.numParcialidad;
                    docRelacionado.idFactura = relacion.idFactura;
                    if (docRelacionado.impPagado > docRelacionado.impSaldoAnt)
                        throw new PaymentGreaterThanBalanceException("El importe de pago no puede ser mayor a el saldo de la factura");
                    docRelacionado.impSaldoInsoluto = docRelacionado.impSaldoAnt - docRelacionado.impPagado;
                    if(docRelacionado.idDocumento == null || docRelacionado.idDocumento.Length <= 0)
                    {
                        throw new InvalidInvoiceTypeException("La factura a la que se quiere asignar el pago no es de tipo Pagos Parciales o Diferidos");
                    }
                    if(docRelacionado.impSaldoAnt == 0.0)
                    {
                        throw new InvoiceAtZeroException("El saldo de la factura a la que se quiere asignar este pago está en 0");
                    }
                }
            }
        }

        private void sendMail(CFDi cfdi)
        {
            Stream xmlStream = StreamBuilder.getStreamFromString(cfdi.xml);
            new PDFbuilder().PDFgenerate(cfdi);
            string pdfPath = File.Exists("C://TOMZA.SYS/cfdi/pdf/reporte" + cfdi.serie + cfdi.folio + ".pdf") ? "C://TOMZA.SYS/cfdi/pdf/reporte" + cfdi.serie + cfdi.folio + ".pdf" : null;
            //if (File.Exists("C://TOMZA.SYS/cfdi/pdf/reporte" + cfdi.emisor.unidadOperativa.razonSocial.rfc + ".pdf"))
            //{
            //    File.Delete("C://TOMZA.SYS/cfdi/pdf/reporte" + cfdi.emisor.unidadOperativa.razonSocial.rfc + ".pdf");
            //}
            cfdi.xml = null;
            cfdi.emisor.certificado = null;
            Thread mailingThread = new Thread(
                delegate ()
                {
                    MailSender.sendMail("Factura electrónica", new string[1] { cfdi.receptor.usuario.correo }, xmlStream, pdfPath);
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

                validarRespuesta(respuestaTimbre);

                cfdi.xml = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(respuestaTimbre.documentoProcesado));
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw new WebServiceCommunicationException("Error desconocido de comunicación");
            }
        }

        private void validarRespuesta(respuestaTimbrado respuesta)
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
