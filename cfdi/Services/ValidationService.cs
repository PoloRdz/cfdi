using cfdi.Exceptions;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class ValidationService
    {
        public void validateTipoRelacion(CFDiRelacionado relacion, string tipoComprobante)
        {
            string error = "";
            switch (relacion.tipoRelacion)
            {
                case "01":
                    if(relacion.tipoComprobante.Equals("T") || relacion.tipoComprobante.Equals("P") || relacion.tipoComprobante.Equals("N"))
                        error = "No es posible registrar notas de crédito con comprobantes de tipo \"Traslado\", \"Pago\" o \"Nómina\"";
                    break;
                case "02":
                    if (relacion.tipoComprobante.Equals("T") || relacion.tipoComprobante.Equals("P") || relacion.tipoComprobante.Equals("N"))
                        error = "No es posible registrar notas de débito con comprobantes de tipo \"Traslado\", \"Pago\" o \"Nómina\"";
                    break;
                case "03":
                    if (relacion.tipoComprobante.Equals("T") || relacion.tipoComprobante.Equals("P") || relacion.tipoComprobante.Equals("N"))
                        error = "No es posible hacer una factura de devolución de mercancía cuando la factura relacionada es de tipo \"Egreso\", \"Pago\" o \"Nómina\"";
                    break;
                case "04":
                    if (tipoComprobante.Equals("N") && !relacion.tipoComprobante.Equals("N"))
                        error = "Solo se puede sustituir un comprobante de tipo \"Nómina\" con otro comprobante de tipo \"Nómina\"";
                    else if (tipoComprobante.Equals("P") && !relacion.tipoComprobante.Equals("P"))
                        error = "Solo se puede sustituir un comprobante de tipo \"Pago\" con otro comprobante de tipo \"Pago\"";
                    else if (tipoComprobante.Equals("T") && !relacion.tipoComprobante.Equals("T"))
                        error = "Solo se puede sustituir un comprobante de tipo \"Pago\" con otro comprobante de tipo \"Pago\"";
                    else if ((tipoComprobante.Equals("I") || tipoComprobante.Equals("E")) && !(tipoComprobante.Equals("I") || tipoComprobante.Equals("E")))
                        error = "Solo se puede sustituir un comprobante de tipo \"Ingreso\" o \"Egreso\" con otro comprobante de tipo \"Ingreso\" o \"Egreso\"";
                    break;
                case "05":
                    if(!tipoComprobante.Equals("T") || !(relacion.tipoComprobante.Equals("I") || relacion.tipoComprobante.Equals("E")))
                        error = "Esta factura debe ser de tipo \"Traslado\" y la factura relacionada debe ser de tipo \"Ingreso\" o \"Egreso\"";
                    break;
                case "06":
                    if (!(tipoComprobante.Equals("I") || tipoComprobante.Equals("E")) || !relacion.tipoComprobante.Equals("T"))
                        error = "Esta factura debe ser de tipo \"Ingreso\" o \"Egreso\" y la factura relacionada debe ser de tipo \"Traslado\"";
                    break;
                case "07":
                    if (!(tipoComprobante.Equals("I") || tipoComprobante.Equals("E")) || !(relacion.tipoComprobante.Equals("I") || relacion.tipoComprobante.Equals("E")))
                        error = "Esta factura debe ser de tipo \"Ingreso\" o \"Egreso\" y la factura relacionada debe ser de tipo \"Ingreso\" o \"Egreso\"";
                    break;
                default:
                    error = "Este tipo de relación no existe, verifica";
                    break;
            }
            if (!error.Equals(""))
            {
                throw new InvalidRelationshipException(error);
            }
        }

        public void validateTipoRelacion(CFDiRelacionado[] relaciones, string tipoComprobante)
        {
            foreach(CFDiRelacionado relacion in relaciones)
            {
                validateTipoRelacion(relacion, tipoComprobante);
            }
        }
    }
}
