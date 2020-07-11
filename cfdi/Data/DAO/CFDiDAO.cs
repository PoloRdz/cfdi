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
            cmd.Parameters.AddWithValue("@PP_K_RAZON_SOCIAL", cfdi.emisor.unidadOperativa.razonSocial.idRazonSocial);
            cmd.Parameters.AddWithValue("@PP_FOLIO_FISCAL", cfdi.folioFiscal == null? "" : cfdi.folioFiscal);
            cmd.Parameters.AddWithValue("@PP_NOMBRE_RECEPTOR", cfdi.receptor.informacionFiscal.razonSocial);
            cmd.Parameters.AddWithValue("@PP_RFC_RECEPTOR", cfdi.receptor.informacionFiscal.rfc);
            cmd.Parameters.AddWithValue("@PP_EMAIL", cfdi.receptor.usuario.correo);
            cmd.Parameters.AddWithValue("@PP_TIPO_COMPROBANTE", "I");//cfdi.tipoCompra.Substring(0, 1));
            cmd.Parameters.AddWithValue("@PP_USO_CFDI", cfdi.usoCFDi);
            cmd.Parameters.AddWithValue("@PP_SERIE", cfdi.emisor.serie == null ? "" : cfdi.emisor.serie);
            cmd.Parameters.AddWithValue("@PP_K_ESTATUS_FACTURA", guardarConceptos? 1 : 3);
            cmd.Parameters.AddWithValue("@PP_FECHA_CERTIFICACION", cfdi.fechaCert);
            cmd.Parameters.AddWithValue("@PP_FECHA_EMISION", cfdi.fecha);
            cmd.Parameters.AddWithValue("@PP_NO_CERTIFICADO_SAT", cfdi.NoCertificadoSat == null ? "" : cfdi.NoCertificadoSat);
            cmd.Parameters.AddWithValue("@PP_FORMA_PAGO", "99");//cfdi.formaPago);
            cmd.Parameters.AddWithValue("@PP_NO_CERTIFICADO_EMISOR", cfdi.NoCertificadoEmisor == null ? "" : cfdi.NoCertificadoEmisor);
            cmd.Parameters.AddWithValue("@PP_METODO_PAGO", "PUE");//cfdi.mPago);
            cmd.Parameters.AddWithValue("@PP_SUB_TOTAL", cfdi.subtotal);
            cmd.Parameters.AddWithValue("@PP_SUBTOTAL_IVA", 0.0);
            cmd.Parameters.AddWithValue("@PP_TOTAL_IVA", cfdi.totalImp);
            cmd.Parameters.AddWithValue("@PP_TOTAL", cfdi.total);
            cmd.Parameters.AddWithValue("@PP_SALDO", 0.0d); //cfdi.mPago == "PPD" ? cfdi.total : 0.0d);
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
                    if (cfdi.pagos != null && cfdi.pagos.doctoRelacionados != null && cfdi.pagos.doctoRelacionados.Length > 0)
                        savePagos(cfdi.pagos, cfdi.idFolio, cmd);
                    saveRelacionados(cfdi.relaciones, cfdi.idFolio, cmd);
                    saveConceptos(cfdi.conceptos, cfdi.idFolio, cmd);
                    
                }
                if (cfdi.folio > 0 && !guardarConceptos && cfdi.pagos != null && cfdi.pagos.doctoRelacionados != null)
                    foreach (DoctoRelacionado docRelacion in cfdi.pagos.doctoRelacionados)
                        updateInvoiceBalance(docRelacion, cmd);
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

        public DoctoRelacionado getDoctoRelacionadoInfo(int folio, string serie)
        {
            DoctoRelacionado docRelacion = new DoctoRelacionado();
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_FACTURA_PAGOS_INFO", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_SERIE", serie);
            cmd.Parameters.AddWithValue("@PP_FOLIO", folio);
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                throw new InvalidInvoiceNumberException("Factura no existe o no es de pagos parciales o diferidos");
            reader.Read();
            docRelacion.idDocumento = reader.GetValue(0).ToString();
            docRelacion.numParcialidad = int.Parse(reader.GetValue(1).ToString());
            docRelacion.impSaldoAnt = double.Parse(reader.GetValue(2).ToString());
            docRelacion.idFactura = int.Parse(reader.GetValue(3).ToString());
            cnn.Dispose();
            reader.Close();
            return docRelacion;
        }

        public void updateInvoiceBalance(DoctoRelacionado docRelacion, SqlCommand cmd)
        {
            cmd.CommandText = "PG_UP_SALDO_FACTURA_INFO";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_K_USUARIO", 0);
            /////////////////////////////////////////////////
            cmd.Parameters.AddWithValue("@PP_ID_FACTURA", docRelacion.idFactura);
            cmd.Parameters.AddWithValue("@PP_SALDO", docRelacion.impSaldoInsoluto);
            cmd.ExecuteNonQuery();
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
            reader.Read();
            cfdi.emisor = new Emisor 
            { 
                unidadOperativa = new UnidadOperativa
                {
                    razonSocial = new RazonSocial
                    {
                        rfc = reader.GetValue(0).ToString()
                    }
                }
            };
            cfdi.receptor = new Receptor 
            { 
                informacionFiscal = new InformacionFiscal
                {
                    rfc = reader.GetValue(1).ToString()
                }
            };
            cfdi.folioFiscal = reader.GetValue(2).ToString();
            cfdi.total = double.Parse(reader.GetValue(3).ToString());
            cfdi.serie = serie;
            cfdi.folio = folio;
            cnn.Dispose();
            reader.Close();
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
            cmd.ExecuteNonQuery();
            int statusCode = (int)cmd.Parameters["@PP_STATUS_CODE"].Value;
            cnn.Dispose();
            if (statusCode == 1)
                return true;
            return false;
        }

        public void saveConceptos(List<Concepto> conceptos, int idFactura, SqlCommand cmd)
        {
            foreach(Concepto concepto in conceptos)
            {
                concepto.idConcepto = saveConcepto(concepto, idFactura, cmd);
                if(concepto.impuestos != null && concepto.impuestos.Count > 0)
                    foreach(Impuesto impuesto in concepto.impuestos)
                        impuesto.idImpuesto = saveConceptoImpuesto(impuesto, concepto.idConcepto, cmd);
            }
        }

        public void savePagos(Pagos pagos, int idFactura, SqlCommand cmd)
        {
            cmd.CommandText = "PG_SV_PAGOS_INFO";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_K_USUARIO", 0);
            /////////////////////////////////////////////////
            cmd.Parameters.AddWithValue("@PP_ID_PAGO", pagos.idPago).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_ID_FACTURA", idFactura);
            cmd.Parameters.AddWithValue("@PP_FECHA_PAGO", pagos.fechaPago);
            cmd.Parameters.AddWithValue("@PP_FORMA_PAGO", pagos.formaDePagoP);
            cmd.Parameters.AddWithValue("@PP_MONEDA", pagos.monedaP);
            cmd.Parameters.AddWithValue("@PP_MONTO", pagos.monto);
            cmd.Parameters.AddWithValue("@PP_CUENTA_BENEFICIARIO", pagos.ctaBeneficiario);
            cmd.Parameters.AddWithValue("@PP_CUENTA_ORDENANTE", pagos.ctaOrdenante);
            cmd.ExecuteNonQuery();
            long id = (long)cmd.Parameters["@PP_ID_PAGO"].Value;
            pagos.idPago = id;
            if (pagos.idPago > 0)
            {
                foreach (DoctoRelacionado docRelacionado in pagos.doctoRelacionados)
                {
                    saveDoctoRelacionado(docRelacionado, pagos.idPago, cmd);
                }
            }
            else throw new Exception("No se ha guardado el complemento de pago");
        }

        public void saveDoctoRelacionado(DoctoRelacionado docRelacionado, long idPago, SqlCommand cmd)
        {
            cmd.CommandText = "PG_SV_DOCTO_REL_INFO";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@PP_L_DEBUG", 0);
            cmd.Parameters.AddWithValue("@PP_K_SISTEMA_EXE", 1);
            cmd.Parameters.AddWithValue("@PP_K_USUARIO", 0);
            /////////////////////////////////////////////////
            cmd.Parameters.AddWithValue("@PP_ID_DOCUMENTO_RELACIONADO", docRelacionado.idDoctoRelacionado).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_ID_PAGO", idPago);
            cmd.Parameters.AddWithValue("@PP_ID_FACTURA", docRelacionado.idFactura);
            cmd.Parameters.AddWithValue("@PP_MONEDA_DR", docRelacionado.monedaDR);
            cmd.Parameters.AddWithValue("@PP_METODO_PAGO", docRelacionado.metodoDePagoDR);
            cmd.Parameters.AddWithValue("@PP_NUMERO_PARCIALIDAD", docRelacionado.numParcialidad);
            cmd.Parameters.AddWithValue("@PP_SALDO_ANTERIOR", docRelacionado.impSaldoAnt);
            cmd.Parameters.AddWithValue("@PP_IMPORTE_PAGADO", docRelacionado.impPagado);
            cmd.Parameters.AddWithValue("@PP_SALDO_INSOLUTO", docRelacionado.impSaldoInsoluto);
            cmd.ExecuteNonQuery();
            long id = (long)cmd.Parameters["@PP_ID_DOCUMENTO_RELACIONADO"].Value;
            if (id > 0)
                docRelacionado.idDoctoRelacionado = id;
            else throw new Exception("No fue posible guardar el complemento Documento relacionado para esta factura");
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
            cmd.Parameters.AddWithValue("@PP_ID_REMISION", concepto.idRemision);
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

        public int GetCFDisCount()
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_FACTURAS_TOTAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_TOTAL_REG", 0).Direction = ParameterDirection.InputOutput;
            int total = 0;
            try
            {
                cmd.ExecuteNonQuery();
                total = (int)cmd.Parameters["@PP_TOTAL_REG"].Value;
                return total;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                cmd.Dispose();
                cnn.Close();
            }
        }

        public List<CFDi> GetCFDis(int pagina, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_FACTURAS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_NUM_PAGINA", pagina);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader rdr = cmd.ExecuteReader();
            var facs = new List<CFDi>();
            try
            {
                if (!rdr.HasRows)
                    throw new NotFoundException("No se han encontrado facturas");
                while (rdr.Read())
                {                    
                    facs.Add(getCFDiFromReader(rdr));
                }
                return facs;
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                rdr.Close();
                cmd.Dispose();
                cnn.Close();
            }
        }

        public List<CFDi> GetMisFacturas(int userId, int pagina, int rpp)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_MIS_FACTURAS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", userId);
            cmd.Parameters.AddWithValue("@PP_NUM_PAGINA", pagina);
            cmd.Parameters.AddWithValue("@PP_RPP", rpp);
            SqlDataReader rdr = cmd.ExecuteReader();
            var facs = new List<CFDi>();
            try
            {
                if (!rdr.HasRows)
                    throw new NotFoundException("No se ha encontrado la factura");
                while (rdr.Read())
                {
                    facs.Add(getCFDiFromReader(rdr));
                }
                return facs;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                rdr.Close();
                cmd.Dispose();
                cnn.Close();
            }
        }

        public CFDi GetFactura(int idFactura)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_FACTURA_DETALLE", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_FACTURA", idFactura);
            SqlDataReader rdr = cmd.ExecuteReader();
            var fac = new CFDi();
            try
            {
                if (!rdr.HasRows)
                    throw new NotFoundException("No se han encontrado facturas");
                while (rdr.Read())
                {
                    fac = getCFDiFromReader(rdr);
                }
                return fac;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                rdr.Close();
                cmd.Dispose();
                cnn.Close();
            }
        }

        public int GetMisFacturasCount(int userId)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_MIS_FACTURAS_TOTAL", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_TOTAL_REG", 0).Direction = ParameterDirection.InputOutput;
            cmd.Parameters.AddWithValue("@PP_ID_USUARIO", userId);
            int total = 0;
            try
            {
                cmd.ExecuteNonQuery();
                total = (int)cmd.Parameters["@PP_TOTAL_REG"].Value;
                return total;
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw e;
            }
            finally
            {
                cmd.Dispose();
                cnn.Close();
            }
        }

        private CFDi getCFDiFromReader(SqlDataReader rdr)
        {
            var fac = new CFDi();
            fac.idFolio = rdr.GetInt32(0);
            fac.serie = rdr.GetValue(1).ToString();
            fac.folio = rdr.GetInt32(2);
            fac.estadoFolio = rdr.GetValue(3).ToString();
            fac.tipoCompra = rdr.GetValue(4).ToString();
            fac.emisor = new Emisor();
            fac.emisor.unidadOperativa = new UnidadOperativa();
            fac.emisor.unidadOperativa.razonSocial = new RazonSocial
            {
                razonSocial = rdr.GetValue(5).ToString(),
                rfc = rdr.GetValue(6).ToString()
            };
            fac.receptor = new Receptor {
                informacionFiscal = new InformacionFiscal
                {
                    razonSocial = rdr.GetValue(7).ToString(),
                    rfc = rdr.GetValue(8).ToString()
                }
            };
            fac.subtotal = double.Parse(rdr.GetValue(9).ToString());
            fac.totalImp = double.Parse(rdr.GetValue(10).ToString());
            fac.total = double.Parse(rdr.GetValue(11).ToString());
            fac.folioFiscal = rdr.GetString(12);
            fac.fechaCert = rdr.GetDateTime(13);
            fac.fecha = rdr.GetDateTime(14);
            fac.NoCertificadoEmisor = rdr.GetString(15);
            return fac;
        }
    }
}
