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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), NotNull]
        public string Name { get; set; }

        public string LogoPath { get; set; }   // ruta local del logo copiado al AppData
        public string Address { get; set; }    // dirección escrita por el usuario

        public double? Latitude { get; set; }  // calculada con geocoding
        public double? Longitude { get; set; }
    }
}
