using cfdi.Data.DAO;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class RazonSocialService
    {
        public Dictionary<string, Object> getRazonesSociales(int pagina, int rpp) {
            var res = new Dictionary<string, Object>();
            var rsDAO = new RazonSocialDAO();
            res.Add("razonesSociales", rsDAO.getRazonesSociales(pagina, rpp));
            res.Add("total", rsDAO.getRazonesSocialesTotal());
            return res;
        }

        public List<RazonSocial> getRazonesSociales()
        {
            var rsDAO = new RazonSocialDAO();
            return rsDAO.getRazonesSociales();
        }

        public RazonSocial getRazonSocial(int idRazonSocial)
        {
            var rsDAO = new RazonSocialDAO();
            return rsDAO.getRazonSocial(idRazonSocial);
        }

        public void insertRazonSocial(RazonSocial rs)
        {
            var rsDAO = new RazonSocialDAO();
            int id = rsDAO.InsertRazonSocial(rs);
            rs.idRazonSocial = id;
        }

        public void updateRazonSocial(RazonSocial rs)
        {
            var rsDAO = new RazonSocialDAO();
            rsDAO.UpdateRazonSocial(rs);
        }

        public void deleteRazonSocial(int idRazonSocial)
        {
            var rsDAO = new RazonSocialDAO();
            rsDAO.DeleteRazonSocial(idRazonSocial);
        }

        public void ActivarRazonSocial(int idRazonSocial)
        {
            var rsDAO = new RazonSocialDAO();
            rsDAO.ActivarRazonSocial(idRazonSocial);
        }
    }
}
