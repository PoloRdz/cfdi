using cfdi.Data.DAO;
using cfdi.Exceptions;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class UsoCFDiService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> getUsoCFDis()
        {
            UsoCFDiDAO usoDAO = new UsoCFDiDAO();
            var res = new Dictionary<string, Object>();
            res.Add("usoCFDis", usoDAO.getUsoCFDisLista());
            return res;
        }

        public UsoCFDi ObtenerUsoCFDi(string usoCFDI)
        {
            UsoCFDiDAO usoCFDiDAO = new UsoCFDiDAO();
            UsoCFDi usoCFDi = null;
            try
            {
                usoCFDi = usoCFDiDAO.ObtenerUsoCFDi(usoCFDI);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (usoCFDi == null)
                throw new NotFoundException("No se ha encontrado el UsoCFDi");
            return usoCFDi;
        }

        public Dictionary<string, Object> ObtenerUsoCFDis(int pagina, int rpp)
        {
            UsoCFDiDAO usoCFDiDAO = new UsoCFDiDAO();
            var resultados = new Dictionary<string, Object>();
            List<UsoCFDi> usoCFDis = null;
            int usoCFDisTotal;
            try
            {
                usoCFDis = usoCFDiDAO.ObtenerUsoCFDis(pagina, rpp);
                usoCFDisTotal = usoCFDiDAO.ObtenerUsoCFDisTotal();
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (usoCFDis == null)
                throw new NotFoundException("No se han encontrado UsoCFDis");

            resultados.Add("UsoCFDis", usoCFDis);
            resultados.Add("total", usoCFDisTotal);
            return resultados;
        }

        public void InsertarUsoCFDi(UsoCFDi usoCFDi)
        {
            UsoCFDiDAO usoCFDiDAO = new UsoCFDiDAO();
            int usoCFDiInsertada = 0;
            try
            {
                usoCFDiInsertada = usoCFDiDAO.InsertarUsoCFDis(usoCFDi);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (usoCFDiInsertada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void ActualizarUsoCFDi(UsoCFDi usoCFDi)
        {
            UsoCFDiDAO usoCFDiDAO = new UsoCFDiDAO();
            int usoCFDiActualizada = 0;
            try
            {
                usoCFDiActualizada = usoCFDiDAO.ActualizarUsoCFDis(usoCFDi);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (usoCFDiActualizada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void EliminarUsoCFDi(string usoCFDI)
        {
            UsoCFDiDAO usoCFDiDAO = new UsoCFDiDAO();
            int usoCFDiEliminada = 0;
            try
            {
                usoCFDiEliminada = usoCFDiDAO.EliminarUsoCFDis(usoCFDI);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (usoCFDiEliminada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }

        public void ActivarUsoCFDi(string usoCFDI)
        {
            UsoCFDiDAO usoCFDiDAO = new UsoCFDiDAO(); ;
            int usoCFDiActivada = 0;
            try
            {
                usoCFDiActivada = usoCFDiDAO.ActivarUsoCFDis(usoCFDI);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                throw new ExcepcionInterna("Error en el servidor", e);
            }
            if (usoCFDiActivada < 1)
                throw new ExcepcionInterna("Error en el servidor");
        }
    }
}
