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
            firmaService = new FirmaSatService("C:\\TOMZA.SYS\\CERTIFICADOS_ERP\\COMPAÑIA IMPORTADORA DE GAS Y PETROLEO DEL GOLFO SA DE CV\\", "IGP160201NK5.cer", "IGP160201NK5.key", "");

            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlElement nodoComprobante = (XmlElement)xml.AppendChild(xml.CreateElement("Comprobante"));
            nodoComprobante.SetAttribute("Version", "3.3");
            nodoComprobante.SetAttribute("Serie", "NG");
            nodoComprobante.SetAttribute("Folio", "3007");
            nodoComprobante.SetAttribute("Fecha", "2019-01-28T15:02:00");
            nodoComprobante.SetAttribute("FormaPago", "99");
            nodoComprobante.SetAttribute("NoCertificado", firmaService.GetCertNumber());
            nodoComprobante.SetAttribute("Certificado", firmaService.GetCertAsString());
            nodoComprobante.SetAttribute("CondicionesDePago", "CREDITO");
            nodoComprobante.SetAttribute("SubTotal", "55591.02");
            nodoComprobante.SetAttribute("Total", "64485.58");
            nodoComprobante.SetAttribute("MetodoPago", "PUE");
            nodoComprobante.SetAttribute("TipoDeComprobante", "E");
            nodoComprobante.SetAttribute("TipoCambio", "1");
            nodoComprobante.SetAttribute("Moneda", "MXN");
            // if has relacionados
            XmlElement nodoRelacionados = (XmlElement)nodoComprobante.AppendChild(xml.CreateElement("CfdiRelacionados"));
            nodoRelacionados.SetAttribute("TipoRelacion", "01");
            XmlElement nodoRelacionado = (XmlElement)nodoRelacionados.AppendChild(xml.CreateElement("CfdiRelacionado"));
            nodoRelacionado.SetAttribute("Rfc", cfdi.rfcSucursal);
            nodoRelacionado.SetAttribute("Nombre", cfdi.sucursal);
            nodoRelacionado.SetAttribute("RegimenFiscal", "601");
            return xml.OuterXml;
        }
    }
}
