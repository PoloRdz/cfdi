using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cfdi.Exceptions;
using cfdi.Models;
using cfdi.Models.DTO;
using cfdi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cfdi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolioController : ControllerBase
    {
        // GET: api/Folio
        [HttpGet]
        public IActionResult Get(int page, int rpp)
        {
            var res = new Dictionary<string, Object>();
            FolioService folSrv = new FolioService();
            try
            {
                res = folSrv.getFolios(page, rpp);
                return Ok(res);
            }
            catch (Exception e)
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

        // GET: api/Folio/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var res = new Dictionary<string, Object>();
            FolioService folSrv = new FolioService();
            try
            {
                res.Add("folio", folSrv.getFolio(id));
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

        // POST: api/Folio
        [HttpPost]
        public IActionResult Post(Folio folio)
        {
            FolioService folSrv = new FolioService();
            var res = new Dictionary<string, Object>();
            try
            {
                folio = folSrv.insertFolio(folio);
                res.Add("folio", folio);
                return Ok(res);
            }
            catch(Exception e)
            {
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // PUT: api/Folio/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Folio folio)
        {
            FolioService folSrv = new FolioService();
            var res = new Dictionary<string, Object>();
            try
            {
                folSrv.updateFolio(folio);
                res.Add("folio", folio);
                return Ok(res);
            }
            catch
            {
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        // POST: api/folio/unidadOperativa
        [HttpPost("unidadOperativa")]
        public IActionResult PostFoliosUnidadOperativa([FromBody]FolioUnidadOperativaDTO[] folios)
        {
            var folioServ = new FolioService();
            var res = new Dictionary<string, Object>();
            try
            {
                res = folioServ.saveFolioUnidadOperativa(folios);
                return Ok(res);
            }
            catch
            {
                res.Add("message", "Error en el servidor");
                return StatusCode(500, res);
            }
        }

        [HttpGet("unidadOperativa/{idRazonSocial}")]
        public IActionResult GetFoliosUnidadOperativa(int idRazonSocial)
        {
            var folioServ = new FolioService();
            var res = new Dictionary<string, Object>();
            try
            {
                res.Add("foliosUnidadOperativa", folioServ.getFoliosUnidadOperativa(idRazonSocial));
                return Ok(res);
            }
            catch(Exception e)
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
    }
}
