using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models.DTO
{
    public class ArchivoCertificado
    {
        public IFormFile certificado { get; set; }
        public IFormFile llave { get; set; }
        public IFormFile logo { get; set; }
    }
}
