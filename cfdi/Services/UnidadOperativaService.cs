using cfdi.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class UnidadOperativaService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, object> getUnidadesOperativasPorZona(int id)
        {
            var res = new Dictionary<string, Object>();
            UnidadOperativaDAO uoDAO = new UnidadOperativaDAO();
            res.Add("unidadesOperativas", uoDAO.GetUnidadOperativasByZonaId(id));
            return res;
        }
    }
}
