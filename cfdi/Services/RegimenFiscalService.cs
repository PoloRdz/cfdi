using cfdi.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class RegimenFiscalService
    {
        public Dictionary<string, Object> GetRegimenesFiscales()
        {
            var res = new Dictionary<string, Object>();
            var rfDAO = new RegimenFiscalDAO();
            res.Add("regimenFiscales", rfDAO.getRegimenesFiscales());
            return res;
        }
    }
}
