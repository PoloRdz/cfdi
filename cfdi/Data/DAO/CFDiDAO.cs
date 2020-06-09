using cfdi.Exceptions;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Data.DAO
{
    public class CFDiDAO
    {
        public void saveCFDI(CFDi cfdi, bool guardarConceptos)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand command = new SqlCommand("PG_SV_CERTCERTIFICADO_INFO", cnn);
            command.Transaction = cnn.BeginTransaction("CfdiTransaction");
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            command.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            command.Parameters.AddWithValue("@PP_K_USUARIO", 0);
            //////////////////////////////////////////////////////////////
            command.Parameters.AddWithValue("@PP_ID_FACTURA", cfdi.idFolio).Direction = ParameterDirection.InputOutput;
            command.Parameters.AddWithValue("@PP_FOLIO", cfdi.folio).Direction = ParameterDirection.InputOutput;
            command.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", cfdi.emisor.idSucursal);
            command.Parameters.AddWithValue("@PP_FOLIO_FISCAL", cfdi.folioFiscal == null? "" : cfdi.folioFiscal);
            command.Parameters.AddWithValue("@PP_NOMBRE_RECEPTOR", cfdi.receptor.nombreReceptor);
            command.Parameters.AddWithValue("@PP_RFC_RECEPTOR", cfdi.receptor.rfcReceptor);
            command.Parameters.AddWithValue("@PP_EMAIL", cfdi.receptor.email);
            command.Parameters.AddWithValue("@PP_USO_CFDI", cfdi.usoCFDi);
            command.Parameters.AddWithValue("@PP_SERIE", cfdi.serie == null ? "" : cfdi.serie);
            command.Parameters.AddWithValue("@PP_K_ESTATUS_FACTURA", 1);
            command.Parameters.AddWithValue("@PP_FECHA_CERTIFICACION", cfdi.fechaCert);
            command.Parameters.AddWithValue("@PP_FECHA_EMISION", cfdi.fecha);
            command.Parameters.AddWithValue("@PP_NO_CERTIFICADO_SAT", cfdi.NoCertificadoSat == null ? "" : cfdi.NoCertificadoSat);
            command.Parameters.AddWithValue("@PP_FORMA_PAGO", cfdi.formaPago);
            command.Parameters.AddWithValue("@PP_NO_CERTIFICADO_EMISOR", cfdi.NoCertificadoEmisor == null ? "" : cfdi.NoCertificadoEmisor);
            command.Parameters.AddWithValue("@PP_METODO_PAGO", cfdi.mPago);
            command.Parameters.AddWithValue("@PP_SUB_TOTAL", cfdi.subtotal);
            command.Parameters.AddWithValue("@PP_SUBTOTAL_IVA", 0.0);
            command.Parameters.AddWithValue("@PP_TOTAL_IVA", cfdi.totalImp);
            command.Parameters.AddWithValue("@PP_TOTAL", cfdi.total);
            command.Parameters.AddWithValue("@PP_IMPORTE_LETRA", cfdi.importeLetra);
            command.Parameters.AddWithValue("@PP_CADENA_CERTIFICADO_SAT", cfdi.selloSat == null ? "" : cfdi.selloSat);
            command.Parameters.AddWithValue("@PP_SELLO_DIGITAL_EMISOR", cfdi.selloEmisor == null ? "" : cfdi.selloEmisor);
            command.Parameters.AddWithValue("@PP_RFC_PROV_CERTIF", cfdi.RfcProvCertif == null ? "" : cfdi.RfcProvCertif);
            command.Parameters.AddWithValue("@PP_XML", cfdi.xml == null ? "" : cfdi.xml);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                cfdi.idFolio = (int)command.Parameters["@PP_ID_FACTURA"].Value;
                cfdi.folio = (int)command.Parameters["@PP_FOLIO"].Value;
                if (cfdi.folio == -1)
                    throw new InvoiceNumberAvailabilityException("No hay números de folio disponibles para timbrar");
                reader.Close();
                if (cfdi.folio > 0 && guardarConceptos)
                {
                    saveConceptos(cfdi.conceptos, cfdi.idFolio, command);
                    saveRelacionados(cfdi.relaciones, cfdi.idFolio, command);
                }                    
                command.Transaction.Commit();
            }
            catch (Exception e)
            {
                command.Transaction.Rollback();
                throw e;
            }
            finally
            {
                cnn.Dispose();
                command.Dispose();
            }
        }

        public void saveConceptos(Concepto[] conceptos, int idFactura, SqlCommand cmd)
        {
            foreach(Concepto concepto in conceptos)
            {
                concepto.idConcepto = saveConcepto(concepto, idFactura, cmd);
                foreach(Impuesto impuesto in concepto.impuestos)
                {
                    impuesto.idImpuesto = saveConceptoImpuesto(impuesto, concepto.idConcepto, cmd);
                }
            }
        }

        private int saveConcepto(Concepto concepto, int idFactura, SqlCommand cmd)
        {
            cmd.CommandText = "PG_SV_CONCEPTO_INFO";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_K_USUARIO", 0);
            /////////////////////////////////////////////////
            cmd.Parameters.AddWithValue("@PP_ID_CONCEPTO", concepto.idConcepto).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_ID_FACTURA", idFactura);
            cmd.Parameters.AddWithValue("@PP_CLAVE_PROD_SERV", concepto.claveProdServ);
            cmd.Parameters.AddWithValue("@PP_N_IDENTIFICACION", concepto.noIdentificacion);
            cmd.Parameters.AddWithValue("@PP_CLAVE_UNIDAD", concepto.claveUnidad);
            cmd.Parameters.AddWithValue("@PP_UNIDAD", concepto.unidad);
            cmd.Parameters.AddWithValue("@PP_CANTIDAD", concepto.cantidad);
            cmd.Parameters.AddWithValue("@PP_DESCRIPCION_PRODUCTO", concepto.descripcion);
            cmd.Parameters.AddWithValue("@PP_P_UNITARIO", concepto.valorUnitario);
            cmd.Parameters.AddWithValue("@PP_IMPORTE", concepto.importe);
            SqlDataReader reader = cmd.ExecuteReader();
            int id = (int)cmd.Parameters["@PP_ID_CONCEPTO"].Value;
            reader.Close();
            return id;
        }

        private int saveConceptoImpuesto(Impuesto impuesto, int idConcepto, SqlCommand cmd)
        {
            cmd.CommandText = "PG_SV_CONCEPTO_IMPUESTO_INFO";
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_K_USUARIO", 0);
            /////////////////////////////////////////////////
            cmd.Parameters.AddWithValue("@PP_ID_IMPUESTO", impuesto.idImpuesto).Direction = ParameterDirection.InputOutput; 
            cmd.Parameters.AddWithValue("@PP_ID_CONCEPTO", idConcepto);
            cmd.Parameters.AddWithValue("@PP_TIPO_IMPUESTO", impuesto.tipo);
            cmd.Parameters.AddWithValue("@PP_BASE", impuesto.precioBase);
            cmd.Parameters.AddWithValue("@PP_IMPUESTO", impuesto.impuesto);
            cmd.Parameters.AddWithValue("@PP_TASA_CUOTA", impuesto.tasaOCuota);
            cmd.Parameters.AddWithValue("@PP_TIPO_FACTOR", impuesto.tipoFactor);
            cmd.Parameters.AddWithValue("@PP_IMPORTE", impuesto.importe);
            SqlDataReader reader = cmd.ExecuteReader();
            int id = (int)cmd.Parameters["@PP_ID_IMPUESTO"].Value;
            reader.Close();
            return id;
        }

        public void saveRelacionados(CFDiRelacionado[] relacionados, int idFactura, SqlCommand cmd)
        {
            foreach(CFDiRelacionado relacion in relacionados)
            {
                cmd.CommandText = "PG_SV_FACTURA_RELACION_INFO";
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
                cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
                cmd.Parameters.AddWithValue("@PP_K_USUARIO", 0);
                /////////////////////////////////////////////////
                cmd.Parameters.AddWithValue("@PP_ID_RELACION", relacion.idRelacion).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue("@PP_ID_FACTURA", idFactura);
                cmd.Parameters.AddWithValue("@PP_TIPO_RELACION", relacion.tipoRelacion);
                cmd.Parameters.AddWithValue("@PP_UUID", relacion.UUID);
                SqlDataReader reader = cmd.ExecuteReader();
                relacion.idRelacion = (int)cmd.Parameters["@PP_ID_RELACION"].Value;
                reader.Close();
            }
        }
    }
}
