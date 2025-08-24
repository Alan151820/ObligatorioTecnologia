using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Data
{
    public class AppDatabase
    {
        public SQLiteAsyncConnection Db { get; }
        public AppDatabase() {
            var path = Path.Combine(FileSystem.AppDataDirectory, "app.db3");
            Db = new SQLiteAsyncConnection(path);
            Db.CreateTableAsync<Modelo.Plan>().Wait();
            Db.CreateTableAsync<Modelo.Sponsor>().Wait();
            Db.CreateTableAsync<Modelo.SponsorPost>().Wait();
        }
    }
}
