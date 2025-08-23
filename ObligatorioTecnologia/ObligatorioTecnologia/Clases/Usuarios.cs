using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ObligatorioTecnologia.Clases
{
    public class Usuario
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string Image { get; set; }

    }
    public class UsuarioDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public UsuarioDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Usuario>().Wait();
        }

        public Task<List<Usuario>> GetUsuariosAsync() =>
            _database.Table<Usuario>().ToListAsync();

        public Task<Usuario> GetUsuarioByEmailAsync(string email) =>
            _database.Table<Usuario>().FirstOrDefaultAsync(u => u.Email == email);

        public Task<int> SaveUsuarioAsync(Usuario usuario) =>
            _database.InsertAsync(usuario);

        public Task<int> UpdateUsuarioAsync(Usuario usuario) =>
            _database.UpdateAsync(usuario);

        public Task<int> DeleteUsuarioAsync(Usuario usuario) =>
            _database.DeleteAsync(usuario);
    }

}
