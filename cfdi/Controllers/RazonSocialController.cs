using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cfdi.Controllers
{
    [Route("api/razon-social")]
    [ApiController]
    [Authorize]
    public class RazonSocialController : ControllerBase
    {
        // GET: api/RazonSocial
        [HttpGet]
        public IActionResult Get(int pagina, int rpp)
        {
            var res = new Dictionary<string, Object>();
            try
            {
                var rsServ = new RazonSocialService();
                res = rsServ.getRazonesSociales(pagina, rpp);
                return Ok(res);
            }
            catch(Exception e)
            {
                if (e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                else
                {
                    res.Add("message", "Error en el servidor");
                    return StatusCode(500, res);
                }
            }
        }

        [HttpGet("todas")]
        public IActionResult GetTodas()
        {
            var res = new Dictionary<string, Object>();
            try
            {
                var rsServ = new RazonSocialService();
                res.Add("razonesSociales", rsServ.getRazonesSociales());
                return Ok(res);
            }
            catch (Exception e)
            {
                if (e is NotFoundException)
                {
                    res.Add("message", e.Message);
                    return NotFound(res);
                }
                else
                {
                    res.Add("message", "Error en el servidor");
                    return StatusCode(500, res);
                }
            }
        }

        // GET: api/RazonSocial/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var res = new Dictionary<string, Object>();
            try
            {
                var rsServ = new RazonSocialService();
                res.Add("razonSocial", rsServ.getRazonSocial(id));
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

        // POST: api/RazonSocial
        [HttpPost]
        public IActionResult Post(RazonSocial razonSocial)
        {
            var res = new Dictionary<string, Object>();
            try
            {
                var rsServ = new RazonSocialService();
                rsServ.insertRazonSocial(razonSocial);
                res.Add("razonSocial", razonSocial);
                return Ok(res);
            }
            catch
            {
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // PUT: api/RazonSocial/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RazonSocial rs)
        {
            rs.idRazonSocial = id;
            var res = new Dictionary<string, Object>();
            try
            {
                var rsServ = new RazonSocialService();
                rsServ.updateRazonSocial(rs);
                return Ok();
            }
            catch (Exception e)
            {
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var res = new Dictionary<string, Object>();
            try
            {
                var rsServ = new RazonSocialService();
                rsServ.deleteRazonSocial(id);
                return Ok();
            }
            catch (Exception e)
            { 
                res.Add("Message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }
    }
}
