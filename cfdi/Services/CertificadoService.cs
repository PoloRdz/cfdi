using cfdi.Models;
using cfdi.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.ConstrainedExecution;

namespace cfdi.Services
{
    public class CertificadoService
    {
        public Dictionary<string, Object> getCertificados(int pagina, int rpp)
        {
            var res = new Dictionary<string, Object>();
            var certDAO = new CertificadoDAO();
            res.Add("certificados", certDAO.GetCertificados(pagina, rpp));
            res.Add("total", certDAO.getCertificadosTotal());
            return res;
        }

        public void insertCertificado(Certificado cert, int idRazonSocial)
        {
            var certDAO = new CertificadoDAO();
            int id = certDAO.InsertCertificado(cert, idRazonSocial);
            cert.idCertificado = id;
        }
        //public Certificado GuardarCertificado(Certificado certificado)
        //{
        //    certificado.fechaExpiracion = diFirmaSAT.

        //}
    }
}
