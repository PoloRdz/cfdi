using cfdi.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class ConceptoService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> GetConceptosFactura(int idFactura)
        {
            var res = new Dictionary<string, Object>();
            var conceptoDAO = new ConceptosDAO();
            res.Add("conceptos", conceptoDAO.GetConceptosFatcura(idFactura));
            return res;
        }
    }
}
