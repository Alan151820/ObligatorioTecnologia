using SQLite;
using ObligatorioTecnologia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Data
{
    public class SponsorDb
    {
        private readonly SQLiteAsyncConnection _db;

        public SponsorDb(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitAsync()
        {
            await _db.CreateTableAsync<Sponsor>();
        }

        public Task<List<Sponsor>> GetAllAsync() =>
            _db.Table<Sponsor>().OrderBy(s => s.Name).ToListAsync();

        public Task<Sponsor> GetAsync(int id) =>
            _db.FindAsync<Sponsor>(id);

        public Task<int> SaveAsync(Sponsor s) =>
            s.Id == 0 ? _db.InsertAsync(s) : _db.UpdateAsync(s);

        public Task<int> DeleteAsync(Sponsor s) =>
            _db.DeleteAsync(s);
    }
}
