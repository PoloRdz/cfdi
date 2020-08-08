using cfdi.Data.DAO;
using cfdi.Exceptions;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class ZonaService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> ObtenerZonasLista()
        {
            ZonaDAO zdao = new ZonaDAO();
            var res = new Dictionary<string, object>();
            res.Add("zonas", zdao.getZonasLista());
            return res;
        }

        public Zona ObtenerZona(int idZona)
        {
            ZonaDAO zonaDAO = new ZonaDAO();
            Zona zona = null;
            try
            {
                zona = zonaDAO.getZona(idZona);
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (zona == null)
                throw new NotFoundException("No se ha encontrado la zona");
            return zona;
        }

        public Dictionary<string, Object> ObtenerZonas(int pagina, int rpp)
        {
            ZonaDAO zonaDAO = new ZonaDAO();
            var resultados = new Dictionary<string, Object>();
            List<Zona> zonas = null;
            int zonasTotal;
            try
            {
                zonas = zonaDAO.ObtenerZonas(pagina, rpp);
                zonasTotal = zonaDAO.ObtenerZonasTotal();
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (zonas == null)
                throw new NotFoundException("No se han encontrado zonas");
            
            resultados.Add("zonas", zonas);
            resultados.Add("total", zonasTotal);
            return resultados;
        }

        public void InsertarZona(Zona zona)
        {
            ZonaDAO zonaDAO = new ZonaDAO();
            int zonaInsertada = 0;
            try
            {
                zonaInsertada = zonaDAO.InsertarZona(zona);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (zonaInsertada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void ActualizarZona(Zona zona)
        {
            ZonaDAO zonaDAO = new ZonaDAO();
            int zonaActualizada = 0;
            try
            {
                zonaActualizada = zonaDAO.ActualizarZona(zona);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (zonaActualizada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void EliminarZona(int idZona)
        {
            ZonaDAO zonaDAO = new ZonaDAO();
            int zonaEliminada = 0;
            try
            {
                zonaEliminada = zonaDAO.EliminarZona(idZona);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (zonaEliminada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }
        public void ActivarZona(int idZona)
        {
            ZonaDAO zonaDAO = new ZonaDAO();
            int zonaActivada = 0;
            try
            {
                zonaActivada = zonaDAO.ActivarZona(idZona);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (zonaActivada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }
    }
}
