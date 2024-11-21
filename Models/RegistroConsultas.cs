using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grupo4FarmaceuticaProyectoMov.Models
{
    public class RegistroConsultas
    {
        public int id {  get; set; }
        public string nombreMedicamento { get; set; }
        public DateTime fechaConsulta { get; set; }
        public string contenido { get; set; }
        public Usuario usuario  { get; set; }

    }
}
