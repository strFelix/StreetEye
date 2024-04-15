using StreetEye.models;

namespace StreetEye.Repository.Utilizadores;
public interface IUtilizadorRepository
{
    public Task<List<Utilizador>> GetAllUtilizadoresAsync();
    public Task<List<Utilizador>> GetAllResponsaveisAsync();
    public Task<Utilizador> GetUtilizadorAsync(int id);
    public Task AddUtilizadorAsync(Utilizador utilizador);
    public void UpdateUtilizadorAsync(Utilizador utilizador);
    public void DeleteUtilizadorAsync(Utilizador utilizador);
}

