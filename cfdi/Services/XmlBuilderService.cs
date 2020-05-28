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

        public FirmaSatService firmaService;

        public string buildXml(CFDi cfdi) {
            firmaService = new FirmaSatService("C:/TOMZA.SYS/CERTIFICADOS_ERP/GYP/", "IGP160201NK5.cer", "IGP160201NK5.key", "IGP160201NK5");

            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlElement nodeComprobante = (XmlElement)xml.AppendChild(xml.CreateElement("Comprobante"));
            nodeComprobante.SetAttribute("Version", "3.3");
            nodeComprobante.SetAttribute("Serie", "NG");
            nodeComprobante.SetAttribute("Folio", "3007");
            nodeComprobante.SetAttribute("Fecha", "2019-01-28T15:02:00");
            nodeComprobante.SetAttribute("FormaPago", "99");
            nodeComprobante.SetAttribute("NoCertificado", firmaService.GetCertNumber());
            nodeComprobante.SetAttribute("Certificado", firmaService.GetCertAsString());
            nodeComprobante.SetAttribute("CondicionesDePago", "CREDITO");
            nodeComprobante.SetAttribute("SubTotal", "55591.02");
            nodeComprobante.SetAttribute("Total", "64485.58");
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
            nodeEmisor.SetAttribute("Rfc", cfdi.rfcSucursal);
            nodeEmisor.SetAttribute("Nombre", cfdi.sucursal);
            nodeEmisor.SetAttribute("RegimenFiscal", "601");
            XmlElement nodeReceptor = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Receptor"));
            nodeReceptor.SetAttribute("Rfc", cfdi.rfcReceptor);
            nodeReceptor.SetAttribute("Nombre", cfdi.nombreReceptor);
            nodeReceptor.SetAttribute("UsoCFDI", "G01");
            XmlElement nodeConceptos = (XmlElement)nodeComprobante.AppendChild(xml.CreateElement("Conceptos"));
            foreach (Concepto concepto in cfdi.conceptos)
            {
                XmlElement nodeConcepto = (XmlElement)nodeConceptos.AppendChild(xml.CreateElement("Concepto"));
                nodeConcepto.SetAttribute("ClaveProdServ", concepto.claveProdServ);
                nodeConcepto.SetAttribute("NoIdentificacion", concepto.noIdentificacion);
                nodeConcepto.SetAttribute("Cantidad", concepto.cantidad);
                nodeConcepto.SetAttribute("ClaveUnidad", concepto.claveUnidad);
                nodeConcepto.SetAttribute("Unidad", concepto.unidad);
                nodeConcepto.SetAttribute("Descripcion", concepto.descripcion);
                nodeConcepto.SetAttribute("ValorUnitario", concepto.valorUnitario);
                nodeConcepto.SetAttribute("Importe", concepto.importe);

            }
            return xml.OuterXml;
        }
    }
}
