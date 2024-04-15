using Microsoft.EntityFrameworkCore;
using StreetEye.data;
using StreetEye.models;

namespace StreetEye.Repository.Utilizadores;
public class UtilizadorRepository : IUtilizadorRepository
{
    private readonly DataContext _context;
    public UtilizadorRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Utilizador>> GetAllUtilizadoresAsync()
    {
        return await _context.Utilizadores.ToListAsync();
    }

    public async Task<List<Utilizador>> GetAllResponsaveisAsync()
    {
        List<Utilizador> list = await _context.Utilizadores
            .Where(u => u.Tipo.Equals(models.enums.TipoUtilizador.RESPONSAVEL))
            .ToListAsync();

        return list;
    }

    public async Task<Utilizador> GetUtilizadorAsync(int id)
    {
        return await _context.Utilizadores.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddUtilizadorAsync(Utilizador utilizador)
    {
        await _context.Utilizadores.AddAsync(utilizador);
        await _context.SaveChangesAsync();
    }

    public async void UpdateUtilizadorAsync(Utilizador utilizador)
    {
        _context.Utilizadores.Update(utilizador);
        await _context.SaveChangesAsync();
    }

    public async void DeleteUtilizadorAsync(Utilizador utilizador)
    {
        _context.Utilizadores.Remove(utilizador);
        await _context.SaveChangesAsync();
    }
}

