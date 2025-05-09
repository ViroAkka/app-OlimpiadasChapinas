using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_OlimpiadasChapinas.Models
{
    public class csEstructuraUsuario
    {
        public class requestUsuario
        {
            public int idUsuario { get; set; }
            public string nombre { get; set; }
            public string apellido { get; set; }
            public string email { get; set; }
            public string contraseña_hash { get; set; }
            public string telefono { get; set; }
            public string DNI { get; set; }
            public string contraseñaAlmacenada { get; set; }
            public string contraseñaActualizada { get; set; }
        }

        public class responseUsuario
        {
            public int respuesta { get; set; }
            public string descripcionRespuesta { get; set; }
        }
    }
}