using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Modelo
{
    public class SponsorPost
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public int SponsorId { get; set; }
        public string Titulo { get; set; } = "";
        public string Contenido { get; set; } = "";
        public DateTime Creado { get; set; } = DateTime.Now;
    }
}
