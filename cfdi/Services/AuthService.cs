using cfdi.Data.DAO;
using cfdi.Exceptions;
using cfdi.Models.Auth;
using cfdi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

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
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            
            UserDAO userDAO = new UserDAO();
            usuario = userDAO.GetUsuario(user.usuario, user.password);
            if (!PasswordHasher.Verify(user.password, usuario.password))
                throw new PasswordMismatchException("La contraseña es incorrecta");
            token = JWT.createToken(usuario, claims);
            dic.Add("usuario", usuario);
            dic.Add("token", token);
            return dic;
        }
    }
}
