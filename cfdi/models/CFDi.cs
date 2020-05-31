using cfdi.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class CFDi
    { 
        public Emisor emisor { get; set; }
        public Receptor receptor { get; set; }
        public string usoCFDi { get; set; }
        public int idFolio { get; set; }
        public int idMov { get; set; }
        public string tipoCompra { get; set; }
        public string tipoVenta { get; set; }
        public string estadoFolio { get; set; }
        public string serie { get; set; }
        public int folio { get; set; }
        public DateTime fecha { get; set; }
        public Concepto[] conceptos { get; set; }
        public double subtotal { get; set; }
        public double totalImp { get; set; }
        public double total { get; set; }
        public string mPago { get; set; }
        public string folioFiscal { get; set; }
        public DateTime fechaCert { get; set; }
        public string xml { get; set; }
    }
}
