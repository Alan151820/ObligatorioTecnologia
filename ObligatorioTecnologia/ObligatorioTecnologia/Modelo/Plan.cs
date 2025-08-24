using SQLite;
using System;
using ObligatorioTecnologia.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Modelo
{
    public class Plan
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public string Beneficios { get; set; } = "";
    }
}
