using cfdi.Data.DAO;
using cfdi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Services
{
    public class SerieService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> getSeries(int page, int rpp)
        {
            SerieDAO serieDAO = new SerieDAO();
            var res = new Dictionary<string, Object>();
            res.Add("series", serieDAO.getSeries(page, rpp));
            res.Add("total", serieDAO.getSeriesCount());
            return res;
        }

        public Serie insertSerie(Serie serie)
        {
            SerieDAO serieDAO = new SerieDAO();
            serie = serieDAO.insertSerie(serie);
            return serie;
        }

        public Serie getSerie(int id)
        {
            SerieDAO serDAO = new SerieDAO();
            Serie serie = serDAO.getSerie(id);
            return serie;
        }

        public void updateSerie(Serie serie)
        {
            SerieDAO serDAO = new SerieDAO();
            serDAO.updateSerie(serie);
        }

        public void deleteSerie(int id)
        {
            SerieDAO serDAO = new SerieDAO();
            serDAO.deleteSerie(id);
        }
    }
}
