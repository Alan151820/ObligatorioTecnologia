using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Modelo
{
    public class Sponsor
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public int PlanID { get; set; }
        public bool PlanPagado { get; set; }
        public string Descripcion { get; set; } = "";
        public string Direccion { get; set; } = "";

        // Ubicación (rellenada al geocodificar):
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
    }
}
