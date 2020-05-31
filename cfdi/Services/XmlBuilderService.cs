using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using cfdi.Models;
using FirmaSAT;

namespace cfdi.Services
{
    public class XmlBuilderService {

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
            firmaService.validateCertExpDate(); //TODO: Falta implementar funcionalidad de validateCertExpDate()
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlElement nodeComprobante = (XmlElement)xml.AppendChild(xml.CreateElement("Comprobante"));
            nodeComprobante.SetAttribute("Version", "3.3");
            nodeComprobante.SetAttribute("Serie", "NG");
            nodeComprobante.SetAttribute("Folio", "3007"); // TODO: generar 
            nodeComprobante.SetAttribute("Fecha", cfdi.fecha.ToString());
            nodeComprobante.SetAttribute("FormaPago", "99"); // TODO: pendiente
            nodeComprobante.SetAttribute("NoCertificado", this.firmaService.GetCertNumber());
            nodeComprobante.SetAttribute("Certificado", this.firmaService.GetCertAsString());
            nodeComprobante.SetAttribute("CondicionesDePago", "CREDITO"); // TODO: pendiente
            nodeComprobante.SetAttribute("SubTotal", cfdi.subtotal.ToString());
            nodeComprobante.SetAttribute("Total", cfdi.total.ToString());
            nodeComprobante.SetAttribute("MetodoPago", "PUE");
            nodeComprobante.SetAttribute("TipoDeComprobante", "E");
            nodeComprobante.SetAttribute("TipoCambio", "1");
            nodeComprobante.SetAttribute("Moneda", "MXN");
            // if has relacionados
            XmlElement nodeRelacionados = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("CfdiRelacionados"));
            nodeRelacionados.SetAttribute("TipoRelacion", "01");
            XmlElement nodeRelacionado = (XmlElement)nodeRelacionados.AppendChild(xml.CreateElement("CfdiRelacionado"));
            nodeRelacionado.SetAttribute("UUID", "9f482db5-d2e6-434d-80c1-bae44ab0ae0f");
            XmlElement nodeEmisor = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Emisor"));
            nodeEmisor.SetAttribute("Rfc", cfdi.emisor.rfcSucursal);
            nodeEmisor.SetAttribute("Nombre", cfdi.emisor.sucursal);
            nodeEmisor.SetAttribute("RegimenFiscal", "601");
            XmlElement nodeReceptor = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Receptor"));
            nodeReceptor.SetAttribute("Rfc", cfdi.receptor.rfcReceptor);
            nodeReceptor.SetAttribute("Nombre", cfdi.receptor.nombreReceptor);
            nodeReceptor.SetAttribute("UsoCFDI", "G01");
            XmlElement nodeConceptos = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Conceptos"));
            foreach (Concepto concepto in cfdi.conceptos)
            {
                XmlElement nodeConcepto = (XmlElement)nodeConceptos.AppendChild(xml.CreateElement("Concepto"));
                nodeConcepto.SetAttribute("ClaveProdServ", concepto.claveProdServ);
                nodeConcepto.SetAttribute("NoIdentificacion", concepto.noIdentificacion);
                nodeConcepto.SetAttribute("Cantidad", concepto.cantidad.ToString("F6"));
                nodeConcepto.SetAttribute("ClaveUnidad", concepto.claveUnidad);
                nodeConcepto.SetAttribute("Unidad", concepto.unidad);
                nodeConcepto.SetAttribute("Descripcion", concepto.descripcion);
                nodeConcepto.SetAttribute("ValorUnitario", concepto.valorUnitario.ToString("F6"));
                nodeConcepto.SetAttribute("Importe", concepto.importe.ToString(""));
                if (concepto.impuestos != null && concepto.impuestos.Length > 0)
                {
                    XmlElement nodeConceptoImpuestos = (XmlElement)nodeConcepto.AppendChild(xml.CreateElement("Impuestos"));
                    foreach(Impuesto impuesto in concepto.impuestos)
                    {
                        string tipoImpuesto = impuesto.tipo == "TRA" ? "Traslados" : "Retenciones";
                        string descripcionImpuesto = impuesto.tipo == "TRA" ? "Traslado" : "Retencion";
                        XmlElement nodeTipoImpuesto = (XmlElement)nodeConceptoImpuestos.AppendChild(xml.CreateElement(tipoImpuesto));
                        XmlElement nodeDescripcionImpuesto = (XmlElement)nodeTipoImpuesto.AppendChild(xml.CreateElement(descripcionImpuesto));
                        setTaxesAttributes(nodeDescripcionImpuesto, impuesto);
                    }                    
                }                
            }
            XmlElement nodeImpuestos = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Impuestos"));
            calculateTotalTaxes(cfdi.conceptos, nodeImpuestos, xml);
            if (this.totalRetenciones > 0.0D)
                nodeImpuestos.SetAttribute("TotalImpuestosRetenidos", this.totalRetenciones.ToString());
            if (this.totalTraslados > 0.0D)
                nodeImpuestos.SetAttribute("TotalImpuestosTrasladados", this.totalTraslados.ToString());

            return xml.OuterXml;
        }

        private void setTaxesAttributes(XmlElement node, Impuesto impuesto, bool addBase = true)
        {
            if(addBase)
                node.SetAttribute("Base", impuesto.precioBase.ToString("F6"));
            node.SetAttribute("Impuesto", impuesto.impuesto);
            node.SetAttribute("TipoFactor", impuesto.tipoFactor);
            node.SetAttribute("TasaOCuota", impuesto.tasaOCuota.ToString("F6"));
            node.SetAttribute("Importe", impuesto.importe.ToString());
        }

        private void calculateTotalTaxes(Concepto[] conceptos, XmlElement nodeImpuestos, XmlDocument xml)
        {
            this.totalRetenciones = 0.0D;
            this.totalTraslados = 0.0D;
            XmlElement nodeTraslados = null;
            XmlElement nodeRetenciones = null;
            foreach(Concepto concepto in conceptos)
            {
                foreach(Impuesto impuesto in concepto.impuestos)
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
    }
}
