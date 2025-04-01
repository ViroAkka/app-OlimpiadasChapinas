using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_OlimpiadasChapinas.Models
{
    public class csEstructuraDeporte
    {
        public class requestDeporte
        {
            public int idDeporte { get; set; }
            public string nombre { get; set; }
            public string categoria { get; set; }
            public string descripcion { get; set; }
            public int cantidadJugadores { get; set; }
        }

        public class requestDeporteByID
        {
            public int? idDeporte { get; set; }
            public string nombre { get; set; }
            public string categoria { get; set; }
            public string descripcion { get; set; }
            public int cantidadJugadores { get; set; }
        }

        public class requestEliminarDeporte
        {
            public int idDeporte { get; set; }
        }

        public class responseDeporte
        {
            public int respuesta { get; set; }
            public string descripcionRespuesta { get; set; }
        }
    }
}