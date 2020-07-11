using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
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
        [Authorize]
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
                return BadRequest(res);
            }
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            var res = new Dictionary<string, Object>();
            UsuarioService usrSrv = new UsuarioService();
            try
            {
                res.Add("usuario", usrSrv.getUsuario(id));
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
                return BadRequest(res);
            }
        }

        // POST: api/Usuario
        [HttpPost]
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
                return BadRequest(res);
            }
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        [Authorize]
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
                return BadRequest(res);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
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
                return BadRequest(res);
            }
        }
    }
}
