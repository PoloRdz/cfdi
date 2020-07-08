using cfdi.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class UsoCFDiService
    {
        public Dictionary<string, Object> getUsoCFDis()
        {
            UsoCFDiDAO usoDAO = new UsoCFDiDAO();
            var res = new Dictionary<string, Object>();
            res.Add("usoCFDis", usoDAO.getUsoCFDis());
            return res;
        }
    }
}
