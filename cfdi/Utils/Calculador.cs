using cfdi.Models;
using Org.BouncyCastle.Crypto.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Utils
{
    public class Calculador
    {

        public void calcularDescuentosConceptos(List<Concepto> conceptos)
        {
            foreach (Concepto concepto in conceptos)
                calcularDescuento(concepto);
        }

        public void calcularDescuento(Concepto concepto)
        {
            if(concepto.descuento > 0.0d)
            {
                concepto.descuento = concepto.descuento * concepto.cantidad;
            }
        }

        public void calcularImpuestoConceptos(List<Concepto> conceptos, UnidadOperativa uo)
        {
            foreach (Concepto concepto in conceptos)
                calcularImpuesto(concepto, uo);
        }
        public void calcularImpuesto(Concepto concepto, UnidadOperativa uo)
        {
            Impuesto impuesto = new Impuesto
            {
                tipo = "002",
                precioBase = concepto.importe,
                impuesto = "RET",
                tipoFactor = "Tasa",
                tasaOCuota = uo.impAplicables,
                importe = concepto.importe * uo.impAplicables
            };
            concepto.impuestos = new List<Impuesto>();
            concepto.impuestos.Add(impuesto);
        }

        public void calcularTotal(CFDi cfdi)
        {
            double subtotal = 0.0d;
            double totalDesc = 0.0d;
            double totalImp = 0.0d;
            foreach(Concepto concepto in cfdi.conceptos)
            {
                subtotal += concepto.importe;
                totalDesc += concepto.descuento;
                foreach (Impuesto impuesto in concepto.impuestos)
                    totalImp += impuesto.importe;
            }
            cfdi.subtotal = subtotal;
            cfdi.totalImp = totalImp;   
            cfdi.total = subtotal - totalDesc + totalImp;
        }

    }
}
