using cfdi.Data.DAO;
using cfdi.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class UsuarioService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> getUsuarios(int page, int rpp)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            var res = new Dictionary<string, Object>();
            res.Add("usuarios", usuarioDAO.getUsuarios(page, rpp));
            res.Add("total", usuarioDAO.getUsuariosCount());
            return res;
        }

        public Usuario insertUsuario(Usuario usuario)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            usuario = usuarioDAO.insertUsuario(usuario);
            return usuario;
        }

        public Usuario getUsuario(int id)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            Usuario usuario = usuarioDAO.getUsuario(id);
            return usuario;
        }

        public void updateUsuario(Usuario usuario)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            usuarioDAO.updateUsuario(usuario);
        }

        public void deleteUsuario(int id)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            usuarioDAO.deleteUsuario(id);
        }
    }
}
