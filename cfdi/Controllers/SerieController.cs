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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SerieController : ControllerBase
    {
        // GET: api/Serie
        [HttpGet]
        public IActionResult Get(int pagina, int rpp)
        {
            var result = new Dictionary<string, Object>();
            SerieService serieSer = new SerieService();
            try
            {
                result = serieSer.getSeries(pagina, rpp);
                return Ok(result);
            } catch(Exception e)
            {
                if(e is NotFoundException)
                {
                    result.Add("message", e.Message);
                    return BadRequest(result);
                }
                result.Add("message", "Error en el servidor");
                return BadRequest(result);
            }
            
        }

        // GET: api/Serie/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var res = new Dictionary<string, Object>();
            SerieService serSrv = new SerieService();
            try
            {
                Serie serie = new Serie();
                serie = serSrv.getSerie(id);
                res.Add("serie", serie);
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

        // POST: api/Serie
        [HttpPost]
        public IActionResult Post(Serie serie)
        {
            SerieService serieSer = new SerieService();
            var res = new Dictionary<string, Object>();
            try
            {
                serie = serieSer.insertSerie(serie);
                res.Add("serie", serie);
                return Ok(res);
            }
            catch (Exception e)
            {
                res.Add("message", "No fue posible guardar la serie");
                return BadRequest(res);
            }
        }

        // PUT: api/Serie/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Serie serie)
        {
            var res = new Dictionary<string, Object>();
            SerieService serSrv = new SerieService();
            try
            {
                serSrv.updateSerie(serie);
                res.Add("serie", serie);
                return Ok(serie);
            }
            catch (Exception e)
            {
                res.Add("message", "Error en el servidor");
                return BadRequest(res);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var res = new Dictionary<string, Object>();
            SerieService serSrv = new SerieService();
            try
            {
                serSrv.deleteSerie(id);
                return Ok();
            }
            catch (Exception e)
            {
                res.Add("message", "Error en el servidor");
                return BadRequest(res);
            }
        }
    }
}
