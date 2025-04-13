using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_OlimpiadasChapinas.Models
{
    public class csEstructuraPago
    {
        public class requestPago
        {
            public int idPago { get; set; }
            public int idFormaPago { get; set; }
            public double montoPago { get; set; }
            public string observaciones { get; set; }
        }


        public class responsePago
        {
            public int respuesta { get; set; }
            public string descripcionRespuesta { get; set; }
        }
    }
}