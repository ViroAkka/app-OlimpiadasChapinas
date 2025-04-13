using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_OlimpiadasChapinas.Models
{
    public class csEstructuraEvento
    {
        public class requestEvento
        {
            public int idEvento { get; set; }
            public int idDeporte { get; set; }
            public int idEventoPadre { get; set; }
            public string nombre { get; set; }
            public string fechaInicio { get; set; }
            public string fechaFin { get; set; }
            public int cantidadParticipantes { get; set; }
            public double montoInscripcion { get; set; }
        }

        public class responseEvento
        {
            public int respuesta { get; set; }
            public string descripcionRespuesta { get; set; }
        }

        public class fecha
        {
            public int day { get; set; }
            public int month { get; set; }
            public int year { get; set; }
        }
    }
}