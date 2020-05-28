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

        //[DllImport("diFirmaSAT2.dll", CharSet = CharSet.Ansi)]
        //private static extern int SAT_GetCertAsString(StringBuilder sbOutput, int nOutChars, string szFileName, int nOptions);

        //public string GetCertAsString()
        //{
        //    StringBuilder stringBuilder = new StringBuilder(0);
        //    int num = SAT_GetCertAsString(stringBuilder, 0, certPathName + certName, 0);
        //    if (num <= 0)
        //    {
        //        return string.Empty;
        //    }
        //    stringBuilder = new StringBuilder(num);
        //    SAT_GetCertAsString(stringBuilder, stringBuilder.Capacity, certPathName + certName, 0);
        //    return stringBuilder.ToString();
        //}
        public string GetCertAsString()
        {
            return FirmaSAT.Sat.GetCertAsString(certPathName + certName);
        }

        //[DllImport("diFirmaSATNet.dll", CharSet = CharSet.Ansi)]
        //private static extern string GetCertNumber(string szFileName);

        //[DllImport("diFirmaSAT2.dll", CharSet = CharSet.Ansi)]
        //private static extern int SAT_GetCertNumber(StringBuilder sbOutput, int nOutChars, string szFileName, int nOptions);

        //public string GetCertNumber()
        //{
        //    StringBuilder stringBuilder = new StringBuilder(0);
        //    int num = SAT_GetCertNumber(stringBuilder, 0, this.certPathName + certName, 0);
        //    if (num <= 0)
        //    {
        //        return string.Empty;
        //    }
        //    stringBuilder = new StringBuilder(num);
        //    SAT_GetCertNumber(stringBuilder, stringBuilder.Capacity, certPathName + certName, 0);
        //    return stringBuilder.ToString();
        //}
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
