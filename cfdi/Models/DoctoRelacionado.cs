using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class DoctoRelacionado
    {
        public long idDoctoRelacionado { get; set; }
        public string idDocumento { get; set; }
        public long idFactura { get; set; }
        public string monedaDR { get; set; }
        public string metodoDePagoDR { get; set; }
        public int numParcialidad { get; set; }
        public double impSaldoAnt { get; set; }
        public double impPagado { get; set; }
        public double impSaldoInsoluto { get; set; }
        public string serie { get; set; }
        public int folio { get; set; }

    }
}
