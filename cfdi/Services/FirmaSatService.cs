using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text;
using cfdi.Exceptions;

namespace cfdi.Services
{
    public class FirmaSatService
    {
        public string certPathName { get; set; }
        public string certName { get; set; }
        public string certKey { get; set; }
        public string privateKey { get; set; }

        public FirmaSatService(string certPathName, string certName, string certKey, string privateKey)
        {
            this.certPathName = certPathName;
            this.certName = certName;
            this.certKey = certKey;
            this.privateKey = privateKey;
        }

        public string GetCertAsString()
        {
            return FirmaSAT.Sat.GetCertAsString(certPathName + certName);
        }

        public string GetCertNumber()
        {
            return FirmaSAT.Sat.GetCertNumber(certPathName + certName);
        }

        public void ValidateCertAndKey() 
        {
            int validation = FirmaSAT.Sat.CheckKeyAndCert(certPathName + certKey, privateKey, certPathName + certName);
            if(validation != 0) throw new InvalidCertificateKeyException("Error al validar la llave privada con contraseña del certificado digital");
        }

        public void validateCertExpDate()
        {
            var now = DateTime.Now;
            var expCert = DateTime.Parse(FirmaSAT.Sat.GetCertExpiry(certPathName + certName));
            if (DateTime.Compare(expCert, now) < 0)
                throw new ExpiredCertificateException("El certificado ha expirado");
        }

        public void ValidateCertAndKeyExistence()
        {
            if (!File.Exists(certPathName + certKey) && !File.Exists(certPathName + certName))
            {
                throw new FileNotFoundException("El certificado o llave que intenta acceder no existe");
            }
        }
    }
}
