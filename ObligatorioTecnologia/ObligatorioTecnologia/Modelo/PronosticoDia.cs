using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Modelo
{
    public class PronosticoDia
    {
        public string Dia { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public string IconoUrl { get; set; }
        public string Descripcion { get; set; }
    }
}
