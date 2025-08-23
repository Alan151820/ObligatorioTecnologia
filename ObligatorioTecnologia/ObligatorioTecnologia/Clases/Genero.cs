using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Clases
{
    public class Genero
    {
        [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
        public List<Genre> Genres { get; set; }
    }

    public partial class Genre
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
}
