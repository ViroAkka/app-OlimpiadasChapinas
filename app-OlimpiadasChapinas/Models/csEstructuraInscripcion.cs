using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_OlimpiadasChapinas.Models
{
    public class csEstructuraInscripcion
    {
        public class requestInscripcion
        {
            public int idEvento { get; set; }
            public int idParticipante { get; set; }
            public int idPago { get; set; }
            public string fuentePublicidad { get; set; }
            public int idEventoActualizado { get; set; }
            public int idParticipanteActualizado { get; set; }
            public int idPagoActualizado { get; set; }
        }

        public class responseInscripcion
        {
            public int respuesta { get; set; }
            public string descripcionRespuesta { get; set; }
        }
    }
}