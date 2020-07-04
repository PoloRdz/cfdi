using cfdi.Data.DAO;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class ZonaService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> getZonas()
        {
            ZonaDAO zdao = new ZonaDAO();
            var res = new Dictionary<string, object>();
            res.Add("zonas", zdao.getZonas());
            return res;
        }
    }
}
