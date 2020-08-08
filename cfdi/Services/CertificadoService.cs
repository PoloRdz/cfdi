using cfdi.Models;
using cfdi.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Http;
using cfdi.Exceptions;
using System.IO;
using cfdi.Models.DTO;
using NLog;
using NLog.LayoutRenderers;

namespace cfdi.Services
{
    public class CertificadoService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Dictionary<string, Object> getCertificados(int pagina, int rpp)
        {
            var res = new Dictionary<string, Object>();
            var certDAO = new CertificadoDAO();
            res.Add("certificados", certDAO.GetCertificados(pagina, rpp));
            res.Add("total", certDAO.getCertificadosTotal());
            return res;
        }

        public Certificado GetCertificado(int idCertificado)
        {
            var certDAO = new CertificadoDAO();
            return certDAO.GetCertificado(idCertificado);
        }

        public void insertCertificado(Certificado cert, int idRazonSocial)
        {
            var certDAO = new CertificadoDAO();
            int id = certDAO.InsertCertificado(cert, idRazonSocial);
            cert.idCertificado = id;
        }
        
        public void actualizarCertificado(Certificado certificado)
        {
            var certDAO = new CertificadoDAO();
            var certificadoViejo = certDAO.GetCertificado(certificado.idCertificado);
            certDAO.UpdateCertificado(certificado);
            try
            {
                //Mover carpeta de certificado
                if(!certificadoViejo.rutaCert.Equals(certificado.rutaCert))
                    Directory.Move(certificadoViejo.rutaCert, certificado.rutaCert);
                //Mover certificado
                if(!(certificado.rutaCert + "/" + certificadoViejo.cert).Equals(certificado.rutaCert + "/" + certificado.cert))
                    File.Move(certificado.rutaCert + "/" + certificadoViejo.cert, certificado.rutaCert + "/" + certificado.cert);
                if(!(certificado.rutaCert + "/" + certificadoViejo.key).Equals(certificado.rutaCert + "/" + certificado.key))
                    File.Move(certificado.rutaCert + "/" + certificadoViejo.key, certificado.rutaCert + "/" + certificado.key);
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
            }
        }

        public void EliminarCertificado(int idCertificado)
        {
            var certDAO = new CertificadoDAO();
            certDAO.RemoveCertificado(idCertificado);
        }

        public void ActivarCertificado(int idCertificado)
        {
            var certDAO = new CertificadoDAO();
            certDAO.ActivarCertificado(idCertificado);
        }

        public bool procesarArchivos(int idCertificado, ArchivoCertificado archivos)
        {
            bool valorRetorno = true;
            var certificadoDAO = new CertificadoDAO();
            var certificado = certificadoDAO.GetCertificado(idCertificado);
            try
            {
                if (!Directory.Exists(certificado.rutaCert))
                    Directory.CreateDirectory(certificado.rutaCert);
                if (archivos.certificado != null)
                {
                    valorRetorno = procesarArchivo(certificado.rutaCert, certificado.cert, archivos.certificado, ".cer");
                    if (valorRetorno)
                    {
                        var firmaSatService = new FirmaSatService(certificado.rutaCert + "\\", certificado.cert, "", "");
                        certificado.fechaExpiracion = firmaSatService.GetCertificateExpiryDate();
                        certificadoDAO.UpdateCertificado(certificado);
                    }
                }
                if (archivos.llave != null)
                    valorRetorno = procesarArchivo(certificado.rutaCert, certificado.key, archivos.llave, ".key");
                if (archivos.logo != null)
                    valorRetorno = procesarArchivo(certificado.rutaCert, "logo_" + certificado.cert.Substring(0, certificado.cert.Length - 4) + ".jpg", archivos.logo, ".jpg");
            }
            catch(Exception e)
            {
                logger.Error(e, e.Message);
            }
            return valorRetorno;
        }

        private bool procesarArchivo(string rutaArchivo, string nombreArchivo, IFormFile archivoCertificado, string extensionArchivo)
        {
            bool valorRetorno = true;
            var extension = archivoCertificado.FileName.Substring(archivoCertificado.FileName.Length - 4);
            if (!extension.Equals(extensionArchivo))
                valorRetorno = false;
            if(valorRetorno)
                valorRetorno = copiarArchivoADisco(rutaArchivo, nombreArchivo, archivoCertificado);
            return valorRetorno;
        }

        private bool copiarArchivoADisco(string rutaArchivo, string nombreArchivo, IFormFile archivoCertificado)
        {
            using (var certificadoFileStream = new FileStream(rutaArchivo + "/" + nombreArchivo, FileMode.Create))
            {
                try
                {
                    archivoCertificado.CopyTo(certificadoFileStream);
                    return true;
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                    return false;
                }
            }
        }
    }
}
