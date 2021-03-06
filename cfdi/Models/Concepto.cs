﻿using iTextSharp.text.pdf.parser.clipper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models
{
    public class Concepto
    {
        public int idConcepto { get; set; }
        public int idRemision { get; set; }
        public int idLinkedServer { get; set; }
        public string claveProdServ { get; set; } // requerido
        public string noIdentificacion { get; set; } // opcional
        public double cantidad { get; set; } // requerido
        public string claveUnidad { get; set; } // requerido
        public string unidad { get; set; } // opcional
        public string descripcion { get; set; } // requerido
        public double valorUnitario { get; set; } // requerido
        public double importe { get; set; } // requerido
        public double descuento { get; set; } // opcional
        public DateTime fecha { get; set; }
        public List<Impuesto> impuestos { get; set; }
    }
}
