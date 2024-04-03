using StreetEye.models;

namespace StreetEye.Repository.Usuarios
{
    public interface IUsuarioRepository
    {
        public Task<List<Usuario>> GetAllAsync();
        public Task<Usuario> GetByUsuarioIdAsync(int id);
        public Task<Usuario> GetByUsuarioEmailAsync(string email);
        public void AddUsuarioAsync(Usuario usuario);
        public void UpdateUsuarioAsync(Usuario usuario);
    }
}
