using cfdi.Data.DAO;
using cfdi.Exceptions;
using cfdi.Models.Auth;
using cfdi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using cfdi.Models;

namespace cfdi.Services
{
    public class AuthService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> authenticateUser(Usuario user) 
        {
            Usuario usuario = null;
            var token = "";
            var dic = new Dictionary<string, object>();
            var claims = new List<Claim>();
            UsuarioDAO userDAO = new UsuarioDAO();
            usuario = userDAO.GetUsuario(user.usuario, user.password);
            var roles = userDAO.getUsuarioRoles(usuario.id);
            foreach (Rol rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol.identificador));
            }
            if (!PasswordHasher.Verify(user.password, usuario.password))
                throw new PasswordMismatchException("La contraseña es incorrecta");
            usuario.password = "*****";
            token = JWT.createToken(usuario, claims);
            dic.Add("usuario", usuario);
            dic.Add("token", token);
            dic.Add("roles", roles);
            return dic;
        }

        public void cambiarContraseña(string username, string contrasena, string contrasenaNueva)
        {
            if (username.Trim() == contrasenaNueva.Trim())
                throw new UnchangedPasswordException("La contraseña nueva no puede se igual a la contraseña actual");
            UsuarioDAO userDAO = new UsuarioDAO();
            contrasenaNueva = PasswordHasher.Hash(contrasenaNueva);
            var usuario = userDAO.GetUsuario(username, contrasena);
            if (!PasswordHasher.Verify(contrasena, usuario.password))
                throw new PasswordMismatchException("La contraseña es incorrecta");
            userDAO.actualizarContrasena(usuario.id, contrasenaNueva);
        }
    }
}
