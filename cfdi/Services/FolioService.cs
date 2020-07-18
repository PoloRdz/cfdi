using cfdi.Data.DAO;
using cfdi.Models;
using cfdi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class FolioService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> getFolios(int page, int rpp)
        {
            FolioDAO folioDAO = new FolioDAO();
            var res = new Dictionary<string, Object>();
            res.Add("folios", folioDAO.getFolios(page, rpp));
            res.Add("total", folioDAO.getFoliosCount());
            return res;
        }

        public Dictionary<string, Object> saveFolioUnidadOperativa (FolioUnidadOperativaDTO[] folios)
        {
            FolioDAO folioDAO = new FolioDAO();
            var res = new Dictionary<string, Object>();
            foreach(FolioUnidadOperativaDTO folio in folios)
            {
                folioDAO.saveFolioUnidadOperativa(folio);
            }
            res.Add("folios", folios);
            return res;
        }

        public Folio insertFolio(Folio folio)
        {
            FolioDAO folioDAO = new FolioDAO();
            folio = folioDAO.insertFolio(folio);
            return folio;
        }

        public Folio getFolio(int id)
        {
            FolioDAO folioDAO = new FolioDAO();
            Folio folio = folioDAO.getFolio(id);
            return folio;
        }

        public void updateFolio(Folio folio)
        {
            FolioDAO folioDAO = new FolioDAO();
            folioDAO.updateFolio(folio);
        }

        public List<FolioUnidadOperativaDTO> getFoliosUnidadOperativa(int idRazonSocial)
        {
            var folioDAO = new FolioDAO();
            return folioDAO.getFoliosUnidadOperativa(idRazonSocial);
        }

        //public void delete(int id)
        //{
        //    SerieDAO serDAO = new SerieDAO();
        //    serDAO.deleteSerie(id);
        //}
    }
}
