using ObligatorioTecnologia.Modelo;
using ObligatorioTecnologia.Data;


namespace ObligatorioTecnologia.Data
{
    public class SponsorRepository
    {
        private readonly AppDatabase _db;
        public SponsorRepository(AppDatabase db) => _db = db;

        public Task<List<Sponsor>> GetAllAsync() => _db.Db.Table<Sponsor>().ToListAsync();
        public Task<Sponsor?> GetAsync(int id) => _db.Db.Table<Sponsor>().Where(s => s.Id == id).FirstOrDefaultAsync();
        public Task<int> SaveAsync(Sponsor s) => s.Id == 0 ? _db.Db.InsertAsync(s) : _db.Db.UpdateAsync(s);
        public Task<int> DeleteAsync(Sponsor s) => _db.Db.DeleteAsync(s);
    }

}
