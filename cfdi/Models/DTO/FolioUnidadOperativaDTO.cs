using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Models.DTO
{
    public class FolioUnidadOperativaDTO
    {
        public int idFolio { get; set; }
        public int idRazonSocial { get; set; }
        public int idUnidadOperativa { get; set; }
        public string unidadOperativa { get; set; }
        public int folios { get; set; }
        public bool usarFoliosCompartidos { get; set; }
        public string mensaje { get; set; }
    }
}
