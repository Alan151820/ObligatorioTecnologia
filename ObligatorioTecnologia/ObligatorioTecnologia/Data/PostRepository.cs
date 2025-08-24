using ObligatorioTecnologia.Modelo;
using ObligatorioTecnologia;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTecnologia.Data;

public class PostRepository
{
    private readonly AppDatabase _db;
    public PostRepository(AppDatabase db) => _db = db;

    public Task<List<SponsorPost>> GetBySponsorAsync(int sponsorId) =>
        _db.Db.Table<SponsorPost>().Where(p => p.SponsorId == sponsorId).OrderByDescending(p => p.Creado).ToListAsync();

    public Task<int> SaveAsync(SponsorPost p) => p.Id == 0 ? _db.Db.InsertAsync(p) : _db.Db.UpdateAsync(p);
}

