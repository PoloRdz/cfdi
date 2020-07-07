using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class CFDi
    {
        public Emisor emisor { get; set; }//si
        public Receptor receptor { get; set; }//si
        public CFDiRelacionado[] relaciones { get; set; }//no
        public string usoCFDi { get; set; }//si
        public int idFolio { get; set; }//si
        public int idMov { get; set; }//si
        public string tipoCompra { get; set; }//si
        public string tipoVenta { get; set; }//si
        public string estadoFolio { get; set; }//si
        public string serie { get; set; }//si
        public int folio { get; set; }//si
        public DateTime fecha { get; set; }//si
        public List<Concepto> conceptos { get; set; }//no
        public Pagos pagos { get; set; }//no
        public double subtotal { get; set; }
        public double totalImp { get; set; }
        public double total { get; set; }
        public double saldo { get; set; }//no
        public string importeLetra { get; set; }//no
        public string mPago { get; set; }
        public string formaPago { get; set; }
        public string moneda { get; set; }
        public string RfcProvCertif { get; set; }
        public string NoCertificadoSat { get; set; }
        public string NoCertificadoEmisor { get; set; }
        public string cadenaCertificadoSat { get; set; }
        public string selloSat { get; set; }
        public string selloEmisor { get; set; }
        public string folioFiscal { get; set; }
        public DateTime fechaCert { get; set; }
        public string xml { get; set; }//no
    }
}
