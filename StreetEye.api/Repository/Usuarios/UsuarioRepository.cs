using Microsoft.EntityFrameworkCore;
using StreetEye.data;
using StreetEye.models;

namespace StreetEye.Repository.Usuarios;
public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly DataContext _context;
    public UsuarioRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario> GetByUsuarioIdAsync(int id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario> GetByUsuarioEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(ue => ue.Email.ToLower() == email.ToLower());
    }

    public async Task AddUsuarioAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async void UpdateUsuarioAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

}

