using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grupo4FarmaceuticaProyectoMov.Models
{
    public class MedicamentoDto
    {
        public int id { get; set; }
        public string nombreMedicamento { get; set; }
        public string presentacion { get; set; }
        public string indicaciones { get; set; }
        public DateTime fechaExpiracion { get; set; }
        public string ubicacionFarmacia { get; set; }
        public decimal precio { get; set; }
    }
}
