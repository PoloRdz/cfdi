using cfdi.Data.DAO;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class RolService
    {

        public List<Rol> GetRoles()
        {
            var rolDAO = new RolDAO();
            return rolDAO.getRoles();
        }
    }
}
