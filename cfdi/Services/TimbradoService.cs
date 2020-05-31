using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Data;
using cfdi.Data.DAO;
using cfdi.models;
using cfdi.Models;

namespace cfdi.Services
{
    public class TimbradoService
    {
        public void Timbrar(CFDi cfdi)
        {
            EmisorDAO emisorDAO = new EmisorDAO();
            cfdi.emisor = emisorDAO.GetIssuerInfo(cfdi.emisor.rfcSucursal);
            cfdi.emisor.certificado = emisorDAO.GetIssuerCertInfo(cfdi.emisor.rfcSucursal);
            XmlBuilderService xmlBuilder = new XmlBuilderService();
            cfdi.xml = xmlBuilder.BuildXml(cfdi);
        }
    }
}
