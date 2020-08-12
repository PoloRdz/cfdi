using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Models.Auth;
using cfdi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // GET: api/Usuario
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Get(int page, int rpp)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                res = usrSrv.getUsuarios(page, rpp);
                return Ok(res);
            }
            catch(Exception e)
            {
                if(e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Get(int id)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                res = usrSrv.getUsuario(id);
                return Ok(res);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // POST: api/Usuario
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Post(Usuario usuario)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                res.Add("usuario", usrSrv.insertUsuario(usuario));
                return Ok(res);
            }
            catch (Exception e)
            {
                if (e is DuplicateUsernameException)
                {
                    res.Add("message", e.Message);
                    return Conflict(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Put(int id, Usuario usuario)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                usrSrv.updateUsuario(usuario);
                res.Add("usuario", usuario);
                return Ok(res);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        [HttpPut("res-cont/{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult ReiniciarContraseña(int id)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                string contrasena = usrSrv.ReiniciarContraseña(id);
                res.Add("password", contrasena);
                return Ok(res);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                usrSrv.deleteUsuario(id);
                return Ok();
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // PUT: api/usuario/activar/5
        [HttpPut("activar/{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Activar(int id)
        {
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                usrSrv.ActivarUsuario(id);
                return Ok();
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    return NotFound(new { message = e.Message });
                }
                return StatusCode(500, new { message = "Error en el servidor" });
            }
        }

        [HttpPost("roles/{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult PostRoles(int id, [FromBody]Rol[] roles)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                usrSrv.GuardarRoles(id, roles);
                return Ok();
            }
            catch (Exception e)
            {
                if (e is DuplicateUsernameException)
                {
                    res.Add("message", e.Message);
                    return Conflict(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        [HttpPut("roles/{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult PutRoles(int id, [FromBody] Rol[] roles)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                usrSrv.ActualizarRoles(id, roles);
                return Ok();
            }
            catch (Exception e)
            {
                if (e is DuplicateUsernameException)
                {
                    res.Add("message", e.Message);
                    return Conflict(res);
                }
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }
    }
}
