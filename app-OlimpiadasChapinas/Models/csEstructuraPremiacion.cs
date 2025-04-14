using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_OlimpiadasChapinas.Models
{
    public class csEstructuraPremiacion
    {
        public class requestPremiacion
        {
            public int idPremiacion { get; set; }
            public int idEvento { get; set; }
            public int idPuesto { get; set; }
            public int idParticipante { get; set; }
        }

        public class responsePremiacion
        {
            public int respuesta { get; set; }
            public string descripcionRespuesta { get; set; }
        }
    }
}