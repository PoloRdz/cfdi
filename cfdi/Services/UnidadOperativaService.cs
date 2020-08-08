using cfdi.Data.DAO;
using cfdi.Exceptions;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class UnidadOperativaService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, object> getUnidadesOperativasPorZona(int id)
        {
            var res = new Dictionary<string, Object>();
            UnidadOperativaDAO uoDAO = new UnidadOperativaDAO();
            res.Add("unidadesOperativas", uoDAO.GetUnidadOperativasByZonaId(id));
            return res;
        }

        public UnidadOperativa ObtenerUnidadOperativa(int idUnidadOperativa)
        {
            UnidadOperativaDAO unidadOperativaDAO = new UnidadOperativaDAO();
            UnidadOperativa unidadOperativa = null;
            try
            {
                unidadOperativa = unidadOperativaDAO.ObtenerUnidadOperativa(idUnidadOperativa);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (unidadOperativa == null)
                throw new NotFoundException("No se ha encontrado la Unidad Operativa");
            return unidadOperativa;
        }

        //public Dictionary<string, Object> ObtenerZonas(int pagina, int rpp)
        //{
        //    UnidadOperativaDAO unidadOperativaDAO = new UnidadOperativaDAO();
        //    var resultados = new Dictionary<string, Object>();
        //    List<UnidadOperativa> unidadesOperativas = null;
        //    int zonasTotal;
        //    try
        //    {
        //        unidadesOperativas = unidadOperativaDAO.ObtenerUnidad(pagina, rpp);
        //        zonasTotal = unidadOperativaDAO.ObtenerZonasTotal();
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e, e.Message);
        //        throw new ExcepcionInterna("Error en el servidor", e);
        //    }
        //    if (unidadesOperativas == null)
        //        throw new NotFoundException("No se han encontrado zonas");

        //    resultados.Add("zonas", unidadesOperativas);
        //    resultados.Add("total", zonasTotal);
        //    return resultados;
        //}

        public void InsertarUnidadOperativa(UnidadOperativa unidadOperativa)
        {
            UnidadOperativaDAO unidadOperativaDAO = new UnidadOperativaDAO();
            int unidadOperativaInsertada = 0;
            try
            {
                unidadOperativaInsertada = unidadOperativaDAO.InsertarUnidadOperativa(unidadOperativa);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (unidadOperativaInsertada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void ActualizarUnidadOperativa(UnidadOperativa unidadOperativa)
        {
            UnidadOperativaDAO unidadOperativaDAO = new UnidadOperativaDAO();
            int unidadOperativaActualizada = 0;
            try
            {
                unidadOperativaActualizada = unidadOperativaDAO.ActualizarUnidadOperativa(unidadOperativa);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (unidadOperativaActualizada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void EliminarUnidadOperativa(int idUnidadOperativa)
        {
            UnidadOperativaDAO unidadOperativaDAO = new UnidadOperativaDAO();
            int unidadOperativaEliminada = 0;
            try
            {
                unidadOperativaEliminada = unidadOperativaDAO.EliminarUnidadOperativa(idUnidadOperativa);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (unidadOperativaEliminada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }
        public void ActivarUnidadOperativa(int idUnidadOperativa)
        {
            UnidadOperativaDAO unidadOperativaDAO = new UnidadOperativaDAO();
            int zonaActivada = 0;
            try
            {
                zonaActivada = unidadOperativaDAO.ActivarUnidadOperativa(idUnidadOperativa);
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
