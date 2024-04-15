using Microsoft.EntityFrameworkCore;
using StreetEye.data;
using StreetEye.models;

namespace StreetEye.Repository.Responsaveis;
public sealed class ResponsavelRepository : IResponsavelRepository
{
    private readonly DataContext _context;
    public ResponsavelRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Responsavel> GetResponsavelAsync(int id)
    {
        return await _context.Responsaveis.FirstOrDefaultAsync(r => r.IdResponsavel == id);
    }

    public async Task<Responsavel> GetByUtilizadorIdAsync(int id)
    {
        return await _context.Responsaveis
            .Include(r => r.ResponsavelUtilizador)
            .Include(r => r.Utilizador)
            .FirstOrDefaultAsync(u => u.IdResponsavel == id);
    }

    public async void AddResponsavelAsync(Responsavel responsavel)
    {
        await _context.Responsaveis.AddAsync(responsavel);
        await _context.SaveChangesAsync();
    }

    public async void DeleteResponsavelAsync(Responsavel responsavel)
    {
        _context.Responsaveis.Remove(responsavel);
        await _context.SaveChangesAsync();
    }
}

