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
    public class ConceptosDAO
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public List<Concepto> getConceptos(int idMov, int idUnidadOperativa)
        {
            SqlConnection cnn = DBConnectionFactory.GetOpenConnection();
            SqlCommand cmd = new SqlCommand("PG_SK_CONCEPTOS_TRANSACCION", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PP_ID_FOLIO_R", idMov);
            cmd.Parameters.AddWithValue("@PP_K_UNIDAD_OPERATIVA", idUnidadOperativa);
            SqlDataReader rdr = null;
            var conceptos = new List<Concepto>();
            try
            {
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new NotFoundException("No se encontraron conceptos para esta transacción");
                while (rdr.Read())
                    conceptos.Add(getConceptoFromReader(rdr));
                return conceptos;
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
                cnn.Dispose();
            }
        }

        private Concepto getConceptoFromReader(SqlDataReader rdr)
        {
            Concepto concepto = new Concepto();
            concepto.claveProdServ = rdr.GetString(0);
            concepto.noIdentificacion = rdr.GetString(1);
            concepto.cantidad = double.Parse(rdr.GetValue(2).ToString());
            concepto.claveUnidad = rdr.GetString(3);
            concepto.unidad = rdr.GetString(4);
            concepto.descripcion = rdr.GetString(5);
            concepto.valorUnitario = double.Parse(rdr.GetValue(6).ToString());
            concepto.importe = double.Parse(rdr.GetValue(7).ToString());
            concepto.descuento = double.Parse(rdr.GetValue(8).ToString());
            return concepto;
        }
    }
}
