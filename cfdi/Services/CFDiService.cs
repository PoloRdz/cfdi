using cfdi.Data.DAO;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class CFDiService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> GetCFDis(int pagina, int totalPag)
        {
            CFDiDAO facDAO = new CFDiDAO();
            var res = new Dictionary<string, Object>();
            res.Add("cfdis", facDAO.GetCFDis(pagina, totalPag));
            res.Add("total", facDAO.GetCFDisCount());
            return res;
        }

    }
}
