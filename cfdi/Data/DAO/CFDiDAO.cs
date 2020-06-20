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
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void saveCFDI(CFDi cfdi, bool guardarConceptos)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SV_CERTCERTIFICADO_INFO", cnn);
            cmd.Transaction = cnn.BeginTransaction("CfdiTransaction");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_K_USUARIO", 0);
            //////////////////////////////////////////////////////////////
            cmd.Parameters.AddWithValue("@PP_ID_FACTURA", cfdi.idFolio).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_FOLIO", cfdi.folio).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", cfdi.emisor.idSucursal);
            cmd.Parameters.AddWithValue("@PP_FOLIO_FISCAL", cfdi.folioFiscal == null? "" : cfdi.folioFiscal);
            cmd.Parameters.AddWithValue("@PP_NOMBRE_RECEPTOR", cfdi.receptor.nombreReceptor);
            cmd.Parameters.AddWithValue("@PP_RFC_RECEPTOR", cfdi.receptor.rfcReceptor);
            cmd.Parameters.AddWithValue("@PP_EMAIL", cfdi.receptor.email);
            cmd.Parameters.AddWithValue("@PP_USO_CFDI", cfdi.usoCFDi);
            cmd.Parameters.AddWithValue("@PP_SERIE", cfdi.emisor.serie == null ? "" : cfdi.emisor.serie);
            cmd.Parameters.AddWithValue("@PP_K_ESTATUS_FACTURA", guardarConceptos? 1 : 3);
            cmd.Parameters.AddWithValue("@PP_FECHA_CERTIFICACION", cfdi.fechaCert);
            cmd.Parameters.AddWithValue("@PP_FECHA_EMISION", cfdi.fecha);
            cmd.Parameters.AddWithValue("@PP_NO_CERTIFICADO_SAT", cfdi.NoCertificadoSat == null ? "" : cfdi.NoCertificadoSat);
            cmd.Parameters.AddWithValue("@PP_FORMA_PAGO", cfdi.formaPago);
            cmd.Parameters.AddWithValue("@PP_NO_CERTIFICADO_EMISOR", cfdi.NoCertificadoEmisor == null ? "" : cfdi.NoCertificadoEmisor);
            cmd.Parameters.AddWithValue("@PP_METODO_PAGO", cfdi.mPago);
            cmd.Parameters.AddWithValue("@PP_SUB_TOTAL", cfdi.subtotal);
            cmd.Parameters.AddWithValue("@PP_SUBTOTAL_IVA", 0.0);
            cmd.Parameters.AddWithValue("@PP_TOTAL_IVA", cfdi.totalImp);
            cmd.Parameters.AddWithValue("@PP_TOTAL", cfdi.total);
            cmd.Parameters.AddWithValue("@PP_IMPORTE_LETRA", cfdi.importeLetra);
            cmd.Parameters.AddWithValue("@PP_CADENA_CERTIFICADO_SAT", cfdi.cadenaCertificadoSat == null ? "" : cfdi.cadenaCertificadoSat);
            cmd.Parameters.AddWithValue("@PP_SELLO_DIGITAL_EMISOR", cfdi.selloEmisor == null ? "" : cfdi.selloEmisor);
            cmd.Parameters.AddWithValue("@PP_SELLO_DIGITAL_SAT", cfdi.selloSat == null ? "" : cfdi.selloSat);
            cmd.Parameters.AddWithValue("@PP_RFC_PROV_CERTIF", cfdi.RfcProvCertif == null ? "" : cfdi.RfcProvCertif);
            cmd.Parameters.AddWithValue("@PP_XML", cfdi.xml == null ? "" : cfdi.xml);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                cfdi.idFolio = (int)cmd.Parameters["@PP_ID_FACTURA"].Value;
                cfdi.folio = (int)cmd.Parameters["@PP_FOLIO"].Value;
                if (cfdi.folio == -1)
                    throw new InvoiceNumberAvailabilityException("No hay números de folio disponibles para timbrar");
                reader.Close();
                if (cfdi.folio > 0 && guardarConceptos)
                {
                    saveConceptos(cfdi.conceptos, cfdi.idFolio, cmd);
                    saveRelacionados(cfdi.relaciones, cfdi.idFolio, cmd);
                }                    
                cmd.Transaction.Commit();
            }
            catch (Exception e)
            {
                logger.Error(e);
                cmd.Transaction.Rollback(); 
                throw e;
            }
            finally
            {
                cnn.Dispose();
                cmd.Dispose();
            }
        }

        public CFDi getInvoiceInfo(string serie, int folio)
        {
            CFDi cfdi = new CFDi();
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_FACTURA_INFO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_SERIE", serie);
            cmd.Parameters.AddWithValue("@PP_FOLIO", folio);
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                throw new InvalidInvoiceNumberException("Serie y/o Folio invalidos");
            cfdi.emisor = new Emisor();
            cfdi.receptor = new Receptor();
            reader.Read();
            cfdi.emisor.rfcSucursal = reader.GetValue(0).ToString();
            cfdi.receptor.rfcReceptor = reader.GetValue(1).ToString();
            cfdi.folioFiscal = reader.GetValue(2).ToString();
            cfdi.total = double.Parse(reader.GetValue(3).ToString());
            cfdi.serie = serie;
            cfdi.folio = folio;
            return cfdi;
        }

        public bool validateInvoiceStatus(string serie, int folio)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_RN_FACTURA_STATUS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_SERIE", serie);
            cmd.Parameters.AddWithValue("@PP_FOLIO", folio);
            cmd.Parameters.AddWithValue("@PP_STATUS_CODE", 0).Direction = ParameterDirection.InputOutput;
            cmd.ExecuteReader();
            int statusCode = (int)cmd.Parameters["@PP_STATUS_CODE"].Value;
            if (statusCode == 1)
                return true;
            return false;
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

        public string getCFDIXml(int idFolio)
        {
            string xml = "";
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_CERT_XML_INFO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_ID_FOLIO", idFolio);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                xml = reader.GetValue(0).ToString();
            }
            reader.Close();
            cnn.Dispose();
            return xml;
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
            if(relacionados != null)
            {
                foreach (CFDiRelacionado relacion in relacionados)
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

        public void cancelarTimbre(CFDi cfdi)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_UP_CANCELAR_FACTURA", cnn);
            cmd.Transaction = cnn.BeginTransaction("CfdiTransaction");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_K_USUARIO", 0);
            /////////////////////////////////////////////////
            cmd.Parameters.AddWithValue("@PP_SERIE", cfdi.serie);
            cmd.Parameters.AddWithValue("@PP_FOLIO", cfdi.folio);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Close();
                cmd.Transaction.Commit();
            }
            catch(Exception e)
            {
                logger.Error(e);
                cmd.Transaction.Rollback();
                throw e;
            }
            finally
            {
                cnn.Dispose();
                cmd.Dispose();
            }
        }
    }
}
