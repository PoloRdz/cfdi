using cfdi.Data.DAO;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Models.Auth;
using cfdi.Utils;
using Microsoft.AspNetCore.Rewrite.Internal.UrlMatches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.ServiceModel.Security;
using System.Text;
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

        public Dictionary<string, Object> getUsuario(int id)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            Usuario usuario = usuarioDAO.getUsuario(id);
            var roles = usuarioDAO.getUsuarioRoles(id);
            var res = new Dictionary<string, Object>();
            res.Add("usuario", usuario);
            res.Add("roles", roles);
            return res;
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

        public void GuardarRoles(int idUsuario, Rol[] roles)
        {
            var usrDAO = new UsuarioDAO();
            foreach(Rol rol in roles)
            {
                usrDAO.GuardarUsuarioRoles(idUsuario, rol);
            }
        }

        public void ActualizarRoles(int idUsuario, Rol[] roles)
        {
            var usuarioDAO = new UsuarioDAO();
            try
            {
                usuarioDAO.EliminarUsuarioRoles(idUsuario);
                foreach (Rol rol in roles)
                {
                    usuarioDAO.GuardarUsuarioRoles(idUsuario, rol);
                }
            }
            catch(Exception e)
            {
                throw new ExcepcionInterna("Error en el servidor", e);
            }
        }

        public void ActivarUsuario(int idUsuario)
        {
            var usuarioDAO = new UsuarioDAO();
            int resultado = 0;
            try
            {
                resultado = usuarioDAO.ActivarUsuario(idUsuario);
            }
            catch(Exception e)
            {
                logger.Error(e);
                throw e;
            }
            if (resultado < 1)
                throw new ExcepcionInterna("No fue posible activar el usuario");
        }

        public string ReiniciarContraseña(int idUsuario)
        {
            var usuarioDAO = new UsuarioDAO();
            StringBuilder contrasenaGenerada = new StringBuilder("P@ssw0rd#");
            string contrasenaEncriptada = "";
            try
            {
                contrasenaGenerada.Append(new Random().Next(999));
                contrasenaEncriptada = PasswordHasher.Hash(contrasenaGenerada.ToString());
                usuarioDAO.actualizarContrasena(idUsuario, contrasenaEncriptada);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw new ExcepcionInterna("Error en el servidor");
            }
            return contrasenaGenerada.ToString();
        }
    }
}
