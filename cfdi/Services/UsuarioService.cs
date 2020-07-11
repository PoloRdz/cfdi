using cfdi.Data.DAO;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Models.Auth;
using cfdi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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

        public InformacionFiscal getInformacionFiscal(int idUsuario)
        {
            var infoFis = new InformacionFiscal();
            var uDAO = new UsuarioDAO();
            infoFis = uDAO.getUsuarioFiscales(idUsuario);
            return infoFis;
        }

        public Usuario insertUsuario(Usuario usuario)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            if (usuarioDAO.buscarNombreUsuario(usuario.usuario))
                throw new DuplicateUsernameException("El nombre de usuario ya existe");
            if (usuarioDAO.buscarCorreo(usuario.correo))
                throw new DuplicateUsernameException("El correo ya existe");
            usuario.password = PasswordHasher.Hash(usuario.password);
            usuario = usuarioDAO.insertUsuario(usuario);
            usuario.password = "******";
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

        public InformacionFiscal insertInformacionFiscal(int idUsuario, InformacionFiscal infoFis)
        {
            UsuarioDAO uDAO = new UsuarioDAO();
            if (infoFis.id == 0 && uDAO.buscarRFC(infoFis.rfc))
                throw new DuplicateRFCException("El RFC ingresado ya existe");
            return uDAO.insertInformacionFiscal(idUsuario, infoFis);
        }
    }
}
