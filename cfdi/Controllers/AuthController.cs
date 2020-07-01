using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using cfdi.Models.Auth;
using cfdi.Utils;
using cfdi.Exceptions;
using cfdi.Services;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login(Usuario user)
        {
            var results = new Dictionary<string, Object>();
            try
            {
                var authService = new AuthService();
                results = authService.authenticateUser(user);
                return Ok(results);
            }
            catch(Exception e)
            {
                if (e is NotFoundException)
                {
                    results.Add("message", e.Message);
                    return NotFound(results);
                }                   
                if (e is PasswordMismatchException)
                {
                    results.Add("message", e.Message);
                    return BadRequest(results);
                }
                results.Add("message", "Error en el servidor");
                return BadRequest(results);
            }
        }

    }
}