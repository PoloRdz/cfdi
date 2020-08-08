using cfdi.Data.DAO;
using cfdi.Exceptions;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class RegimenFiscalService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> GetRegimenesFiscales()
        {
            var res = new Dictionary<string, Object>();
            var rfDAO = new RegimenFiscalDAO();
            res.Add("regimenFiscales", rfDAO.getRegimenesFiscalesLista());
            return res;
        }

        public RegimenFiscal ObtenerRegimenFiscal(int idRegimenFiscal)
        {
            RegimenFiscalDAO regimenFiscalDAO = new RegimenFiscalDAO();
            RegimenFiscal regimenFiscal = null;
            try
            {
                regimenFiscal = regimenFiscalDAO.ObtenerRegimenFiscal(idRegimenFiscal);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (regimenFiscal == null)
                throw new NotFoundException("No se ha encontrado el Régimen Fiscal");
            return regimenFiscal;
        }

        public Dictionary<string, Object> ObtenerRegimenesFiscales(int pagina, int rpp)
        {
            RegimenFiscalDAO regimenFiscalDAO = new RegimenFiscalDAO();
            var resultados = new Dictionary<string, Object>();
            List<RegimenFiscal> regimenesFiscales = null;
            int regimenFiscalTotal;
            try
            {
                regimenesFiscales = regimenFiscalDAO.ObtenerRegimenesFiscales(pagina, rpp);
                regimenFiscalTotal = regimenFiscalDAO.ObtenerRegimenesFiscalesTotal();
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (regimenesFiscales == null)
                throw new NotFoundException("No se han encontrado zonas");

            resultados.Add("regimenesFiscales", regimenesFiscales);
            resultados.Add("total", regimenFiscalTotal);
            return resultados;
        }

        public void InsertarRegimenFiscal(RegimenFiscal regimenFiscal)
        {
            RegimenFiscalDAO regimenFiscalDAO = new RegimenFiscalDAO();
            int regimenFiscalInsertada = 0;
            try
            {
                regimenFiscalInsertada = regimenFiscalDAO.InsertarRegimenFiscal(regimenFiscal);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (regimenFiscalInsertada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void ActualizarRegimenFiscal(RegimenFiscal regimenFiscal)
        {
            RegimenFiscalDAO regimenFiscalDAO = new RegimenFiscalDAO();
            int regimenFiscalActualizada = 0;
            try
            {
                regimenFiscalActualizada = regimenFiscalDAO.ActualizarRegimenFiscal(regimenFiscal);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (regimenFiscalActualizada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void EliminarRegimenFiscal(int idRegimenFiscal)
        {
            RegimenFiscalDAO regimenFiscalDAO = new RegimenFiscalDAO();
            int regimenFiscalEliminada = 0;
            try
            {
                regimenFiscalEliminada = regimenFiscalDAO.EliminarRegimenFiscal(idRegimenFiscal);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (regimenFiscalEliminada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void ActivarRegimenFiscal(int idRegimenFiscal)
        {
            RegimenFiscalDAO unidadOperativaDAO = new RegimenFiscalDAO();
            int regimenFiscalActivada = 0;
            try
            {
                regimenFiscalActivada = unidadOperativaDAO.ActivarRegimenFiscal(idRegimenFiscal);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (regimenFiscalActivada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }
    }
}
