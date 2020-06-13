using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models.DTO
{
    public class CFDiDTO
    {
        public string folioFiscal { get; set; }
        public string rfcEmisor { get; set; }
        public string rfcReceptor { get; set; }
        public double total { get; set; }
    }
}
