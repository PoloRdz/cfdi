using cfdi.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace cfdi.Utils
{
    public class JWT
    {
        public static string createToken(Usuario user, List<Claim> claims)
        {
            string securityKey = "524a3844335458354464426c46565777775a6f73_GRUPO_TOMZA";
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            claims.Add(new Claim("user", user.usuario));

            var token = new JwtSecurityToken
                (
                    issuer: "TomzaGrpFacturacion",
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: signingCredentials,
                    claims: claims
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
