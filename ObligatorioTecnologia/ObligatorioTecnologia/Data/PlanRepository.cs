using ObligatorioTecnologia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Data
{
    public class PlanRepository
    {
        private readonly AppDatabase _db;
        public PlanRepository(AppDatabase db) => _db = db;

        public Task<List<Plan>> GetAllAsync() => _db.Db.Table<Plan>().ToListAsync();
        public Task<int> SaveAsync(Plan p) => p.Id == 0 ? _db.Db.InsertAsync(p) : _db.Db.UpdateAsync(p);
    }
}
