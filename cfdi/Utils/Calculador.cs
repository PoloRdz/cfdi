using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Utils
{
    public class Calculador
    {

        public void calcularDescuento(Concepto concepto)
        {
            if(concepto.descuento > 0.0d)
            {
                concepto.descuento = concepto.descuento * concepto.cantidad;
            }
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
            concepto.impuestos = Array.Empty<Impuesto>();
            concepto.impuestos[0] = impuesto;
        }

    }
}
