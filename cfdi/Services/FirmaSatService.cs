using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text;

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
            if(validation != 0) throw new Exception("Error al validar la llave privada con contraseña y certificado digital");
        }

        public void validateCertExpDate()
        {
            
        }
    }
}
