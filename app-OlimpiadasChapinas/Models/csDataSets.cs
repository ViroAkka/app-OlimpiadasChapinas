using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace app_OlimpiadasChapinas.Models
{
    public class csDataSets
    {
        public DataSet Data { get; set; }
        public DataSet Deporte { get; set; }
        public DataSet Evento { get; set; }
        public DataSet FormaPago{ get; set; }
        public DataSet Inscripcion { get; set; }
        public DataSet Pago { get; set; }
        public DataSet Pais { get; set; }
        public DataSet Participante { get; set; }
        public DataSet Premiacion{ get; set; }
        public DataSet PuestoPremiacion{ get; set; }
        public DataSet Usuario { get; set; }
    }
}