using cfdi.Models;
using cfdi.Models.DTO;
using cfdi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace cfdi.Utils
{
    public class CfdiXmlBuilder
    {
        private FirmaSatService firmaService;
        private double totalRetenciones;
        private double totalTraslados;

        public string BuildXml(CFDi cfdi)
        {
            firmaService = new FirmaSatService(
                cfdi.emisor.certificado.rutaCert + "\\",
                cfdi.emisor.certificado.cert,
                cfdi.emisor.certificado.key,
                cfdi.emisor.certificado.contrasena
            );
            firmaService.ValidateCertAndKey();
            firmaService.validateCertExpDate();
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlElement nodeComprobante = (XmlElement)xml.AppendChild(xml.CreateElement("Comprobante"));
            nodeComprobante.SetAttribute("version", "3.3");
            nodeComprobante.SetAttribute("serie", cfdi.emisor.serie); 
            nodeComprobante.SetAttribute("folio", cfdi.folio.ToString()); 
            nodeComprobante.SetAttribute("fecha", cfdi.fecha.ToString("yyyy-MM-ddTHH:mm:ss"));
            if(cfdi.tipoCompra.Substring(0, 1) != "P")
                nodeComprobante.SetAttribute("FormaPago", cfdi.formaPago); // TODO: 
            nodeComprobante.SetAttribute("CondicionesDePago", cfdi.tipoVenta); // TODO: pendiente
            nodeComprobante.SetAttribute("TipoDeComprobante", cfdi.tipoCompra.Substring(0, 1));
            if (cfdi.tipoCompra.Substring(0, 1) != "P")
                nodeComprobante.SetAttribute("MetodoPago", cfdi.mPago); //
            nodeComprobante.SetAttribute("LugarExpedicion", cfdi.emisor.codigoPostal);
            nodeComprobante.SetAttribute("Moneda", cfdi.moneda);
            if (cfdi.tipoCompra.Substring(0, 1) != "P")
                nodeComprobante.SetAttribute("TipoCambio", "1");
            nodeComprobante.SetAttribute("SubTotal", cfdi.subtotal.ToString("F2"));
            nodeComprobante.SetAttribute("Total", cfdi.total.ToString("F2"));            
            nodeComprobante.SetAttribute("Sello", "xxx");                       
            nodeComprobante.SetAttribute("Certificado", this.firmaService.GetCertAsString());
            nodeComprobante.SetAttribute("NoCertificado", this.firmaService.GetCertNumber());
            // if tiene relacionados
            if (cfdi.relaciones != null && cfdi.relaciones.Length > 0)
            {
                foreach(CFDiRelacionado relacion in cfdi.relaciones)
                {
                    XmlElement nodeRelacionados = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("CfdiRelacionados"));
                    nodeRelacionados.SetAttribute("TipoRelacion", relacion.tipoRelacion);
                    XmlElement nodeRelacionado = (XmlElement)nodeRelacionados.AppendChild(xml.CreateElement("CfdiRelacionado"));
                    nodeRelacionado.SetAttribute("UUID", relacion.UUID);
                }                
            }
            XmlElement nodeEmisor = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Emisor"));
            nodeEmisor.SetAttribute("Rfc", cfdi.emisor.rfcSucursal);
            nodeEmisor.SetAttribute("Nombre", cfdi.emisor.sucursal);
            nodeEmisor.SetAttribute("RegimenFiscal", cfdi.emisor.regimenFiscal);
            XmlElement nodeReceptor = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Receptor"));            
            nodeReceptor.SetAttribute("Nombre", cfdi.receptor.nombreReceptor);
            nodeReceptor.SetAttribute("Rfc", cfdi.receptor.rfcReceptor);
            nodeReceptor.SetAttribute("UsoCFDI", cfdi.usoCFDi); 
            if (cfdi.receptor.email != null && cfdi.receptor.email != "")
                nodeReceptor.SetAttribute("To", cfdi.receptor.email);
            XmlElement nodeConceptos = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Conceptos"));
            foreach (Concepto concepto in cfdi.conceptos)
            {
                XmlElement nodeConcepto = (XmlElement)nodeConceptos.AppendChild(xml.CreateElement("Concepto"));
                nodeConcepto.SetAttribute("ClaveProdServ", concepto.claveProdServ);
                nodeConcepto.SetAttribute("NoIdentificacion", concepto.noIdentificacion);
                nodeConcepto.SetAttribute("ClaveUnidad", concepto.claveUnidad);
                nodeConcepto.SetAttribute("Unidad", concepto.unidad);
                nodeConcepto.SetAttribute("Descripcion", concepto.descripcion);
                nodeConcepto.SetAttribute("Cantidad", concepto.cantidad.ToString("F6"));
                nodeConcepto.SetAttribute("ValorUnitario", concepto.valorUnitario.ToString("F6"));
                nodeConcepto.SetAttribute("Importe", concepto.importe.ToString("F2"));
                if (concepto.impuestos != null && concepto.impuestos.Length > 0)
                {
                    XmlElement nodeConceptoImpuestos = (XmlElement)nodeConcepto.AppendChild(xml.CreateElement("Impuestos"));
                    foreach (Impuesto impuesto in concepto.impuestos)
                    {
                        string tipoImpuesto = impuesto.tipo == "TRA" ? "Traslados" : "Retenciones";
                        string descripcionImpuesto = impuesto.tipo == "TRA" ? "Traslado" : "Retencion";
                        XmlElement nodeTipoImpuesto = (XmlElement)nodeConceptoImpuestos.AppendChild(xml.CreateElement(tipoImpuesto));
                        XmlElement nodeDescripcionImpuesto = (XmlElement)nodeTipoImpuesto.AppendChild(xml.CreateElement(descripcionImpuesto));
                        setTaxesAttributes(nodeDescripcionImpuesto, impuesto);
                    }
                }
            }
            XmlElement nodeImpuestos = xml.CreateElement("Impuestos");
            calculateTotalTaxes(cfdi.conceptos, nodeImpuestos, xml);
            if (this.totalRetenciones > 0.0D)
                nodeImpuestos.SetAttribute("TotalImpuestosRetenidos", this.totalRetenciones.ToString("F2"));
            if (this.totalTraslados > 0.0D)
                nodeImpuestos.SetAttribute("TotalImpuestosTrasladados", this.totalTraslados.ToString("F2"));
            if (this.totalRetenciones > 0.0D || this.totalTraslados > 0.0D)
                nodeComprobante.AppendChild(nodeImpuestos);

            if(cfdi.pagos != null && cfdi.pagos.doctoRelacionados != null && cfdi.pagos.doctoRelacionados.Length > 0)
            {
                XmlElement nodeComplemento = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Complemento"));
                XmlElement nodePagos = (XmlElement)nodeComplemento.AppendChild(xml.CreateElement("Pagos"));
                nodePagos.SetAttribute("Version", "1.0");
                XmlElement nodePago = (XmlElement)nodePagos.AppendChild(xml.CreateElement("Pago"));
                nodePago.SetAttribute("FechaPago", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                nodePago.SetAttribute("FormaDePagoP", cfdi.pagos.formaDePagoP);
                nodePago.SetAttribute("MonedaP", cfdi.pagos.monedaP);
                nodePago.SetAttribute("NumOperacion", cfdi.pagos.numOperacion);
                nodePago.SetAttribute("CtaBeneficiario", cfdi.pagos.ctaBeneficiario);
                nodePago.SetAttribute("CtaOrdenante", cfdi.pagos.ctaOrdenante);
                foreach(DoctoRelacionado doctRelacion in cfdi.pagos.doctoRelacionados)
                {
                    XmlElement nodeDoctoRelacionado = (XmlElement)nodePago.AppendChild(xml.CreateElement("DoctoRelacionado"));
                    nodeDoctoRelacionado.SetAttribute("IdDocumento", doctRelacion.idDocumento);
                    nodeDoctoRelacionado.SetAttribute("MonedaDR", doctRelacion.modedaDR);
                    nodeDoctoRelacionado.SetAttribute("MetodoDePagoDR", doctRelacion.metodoDePagoDR);
                    nodeDoctoRelacionado.SetAttribute("NumParcialidad", doctRelacion.numParcialidad.ToString());
                    nodeDoctoRelacionado.SetAttribute("ImpSaldoAnt", doctRelacion.impSaldoAnt.ToString("F2"));
                    nodeDoctoRelacionado.SetAttribute("ImpPagado", doctRelacion.impPagado.ToString("F2"));
                    nodeDoctoRelacionado.SetAttribute("ImpSaldoInsoluto", doctRelacion.impSaldoInsoluto.ToString("F2"));
                    nodeDoctoRelacionado.SetAttribute("Serie", doctRelacion.serie);
                    nodeDoctoRelacionado.SetAttribute("Folio", doctRelacion.folio.ToString());
                }
            }
            

            // Datos Dummy, son requeridos por PAC pero no son procesados ¿¿¿¿¿¿¿¿¿¿¿¿¿¿¿??????????????
            XmlElement nodeDatosVarios = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("DatosVarios"));
            XmlElement nodeSucursal = (XmlElement)nodeDatosVarios.AppendChild(xml.CreateElement("Sucursal"));
            nodeSucursal.SetAttribute("texto", cfdi.emisor.sucursal);
            XmlElement nodeMsgMedio = (XmlElement)nodeDatosVarios.AppendChild(xml.CreateElement("MsgMedio"));
            nodeMsgMedio.SetAttribute("texto", "FECHA LIMITE DE PAGO");
            XmlElement nodeMsgCorto = (XmlElement)nodeDatosVarios.AppendChild(xml.CreateElement("MsgCorto"));
            nodeMsgCorto.SetAttribute("texto", "Cant: 0");
            XmlElement nodeZona = (XmlElement)nodeDatosVarios.AppendChild(xml.CreateElement("Zona"));
            nodeZona.SetAttribute("texto", "Z: AC");
            XmlElement nodeSupervision = (XmlElement)nodeDatosVarios.AppendChild(xml.CreateElement("Supervision"));
            nodeSupervision.SetAttribute("texto", "S: 1");
            XmlElement nodeGrupo = (XmlElement)nodeDatosVarios.AppendChild(xml.CreateElement("Grupo"));
            nodeGrupo.SetAttribute("texto", "C: RAUL ESPEJEL");
            XmlElement nodeMsgCorto2 = (XmlElement)nodeDatosVarios.AppendChild(xml.CreateElement("MsgCorto2"));
            nodeMsgCorto2.SetAttribute("texto", "ESTE RECIBO UNICAMENTE SERA VALIDO COMO PAGO SI PRESENTA EL COMPROBANTE QUE AMPARE EL IMPORTE DEL MISMO EFECTOS FISCALES AL PAGO");
            XmlElement nodeMsgCorto3 = (XmlElement)nodeDatosVarios.AppendChild(xml.CreateElement("MsgCorto3"));
            nodeMsgCorto3.SetAttribute("texto", "CP: " + cfdi.emisor.codigoPostal);

            return xml.OuterXml;
        }

        public string BuildCancelacionXml(CFDi cfdi)
        {
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlElement nodeCancelacion = (XmlElement)xml.AppendChild(xml.CreateElement("Cancelacion"));
            nodeCancelacion.SetAttribute("RfcEmisor", cfdi.emisor.rfcSucursal);
            nodeCancelacion.SetAttribute("Fecha", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            nodeCancelacion.SetAttribute("RfcReceptor", cfdi.receptor.rfcReceptor);
            nodeCancelacion.SetAttribute("Total", cfdi.total.ToString("F2"));
            XmlElement nodeFolios = (XmlElement)nodeCancelacion.AppendChild(xml.CreateElement("Folios"));
            XmlElement nodeUUID = (XmlElement)nodeFolios.AppendChild(xml.CreateElement("UUID"));
            nodeUUID.InnerText = cfdi.folioFiscal;
            return xml.OuterXml;
        }        

        private void setTaxesAttributes(XmlElement node, Impuesto impuesto, bool addBase = true)
        {
            node.SetAttribute("Impuesto", impuesto.impuesto);
            node.SetAttribute("TipoFactor", impuesto.tipoFactor);
            if (addBase)
                node.SetAttribute("Base", impuesto.precioBase.ToString("F6"));
            node.SetAttribute("TasaOCuota", impuesto.tasaOCuota.ToString("F6"));
            node.SetAttribute("Importe", impuesto.importe.ToString("F2"));
        }

        private void calculateTotalTaxes(Concepto[] conceptos, XmlElement nodeImpuestos, XmlDocument xml)
        {
            this.totalRetenciones = 0.0D;
            this.totalTraslados = 0.0D;
            XmlElement nodeTraslados = null;
            XmlElement nodeRetenciones = null;
            foreach (Concepto concepto in conceptos)
            {
                foreach (Impuesto impuesto in concepto.impuestos)
                {
                    if (impuesto.tipo == "TRA")
                    {
                        this.totalTraslados += impuesto.importe;
                        if (nodeTraslados == null)
                            nodeTraslados = (XmlElement)nodeImpuestos.AppendChild(xml.CreateElement("Traslados"));
                        XmlElement nodeImpuesto = (XmlElement)nodeTraslados.AppendChild(xml.CreateElement("Traslado"));
                        setTaxesAttributes(nodeImpuesto, impuesto, false);
                    }
                    if (impuesto.tipo == "RET")
                    {
                        this.totalRetenciones += impuesto.importe;
                        if (nodeRetenciones == null)
                            nodeRetenciones = (XmlElement)nodeImpuestos.AppendChild(xml.CreateElement("Retenciones"));
                        XmlElement nodeRetencion = (XmlElement)nodeRetenciones.AppendChild(xml.CreateElement("Retencion"));
                        setTaxesAttributes(nodeRetencion, impuesto, false);
                    }
                }
            }
        }

        public void obtenerDatosTimbre(CFDi cfdi)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(cfdi.xml);

            XmlNodeList nodeList = xml.ChildNodes;
            foreach(XmlNode node in nodeList)
            {
                string nombre = node.Name;
                if(nombre == "cfdi:Comprobante")
                {
                    cfdi.selloEmisor = node.CreateNavigator().GetAttribute("Sello", "");
                    cfdi.NoCertificadoEmisor = node.CreateNavigator().GetAttribute("NoCertificado", "");
                    foreach(XmlNode childNodeComprobante in node.ChildNodes)
                    {
                        if(childNodeComprobante.Name == "cfdi:Complemento")
                        {
                            foreach (XmlNode timbreNode in childNodeComprobante.ChildNodes)
                            {
                                if(timbreNode.Name == "tfd:TimbreFiscalDigital")
                                {
                                    cfdi.folioFiscal = timbreNode.Attributes["UUID"].InnerXml;
                                    cfdi.selloSat = timbreNode.Attributes["SelloSAT"].InnerXml;
                                    cfdi.NoCertificadoSat = timbreNode.Attributes["NoCertificadoSAT"].InnerXml;
                                    cfdi.RfcProvCertif = timbreNode.Attributes["RfcProvCertif"].InnerXml;
                                    StringBuilder cadena = new StringBuilder();
                                    cadena.Append("||").Append(timbreNode.Attributes["Version"].InnerText).Append("|")
                                          .Append(cfdi.folioFiscal).Append("|")
                                          .Append(timbreNode.Attributes["FechaTimbrado"].InnerText).Append("|")
                                          .Append(timbreNode.Attributes["SelloCFD"].InnerText).Append("|")
                                          .Append(timbreNode.Attributes["NoCertificadoSAT"].InnerText).Append("||");
                                    cfdi.cadenaCertificadoSat = cadena.ToString();
                                }                                
                            }
                        }                        
                    }
                } 
            }
        }
    }
}
