﻿using Microsoft.EntityFrameworkCore;
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

    public async Task AddHistoricoUsuarioAsync(HistoricoUsuario historicoUsuario)
    {
        // Verifique se o usuário existe
        var usuarioExiste = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == historicoUsuario.IdUsuario);
        if (usuarioExiste == null)
        {
            throw new Exception("Usuário não encontrado");
        }

        historicoUsuario.Momento.AddTicks(1);

        // Adicione o histórico
        await _context.HistoricoUsuarios.AddAsync(historicoUsuario);
        await _context.SaveChangesAsync();
    }

    public async void UpdateUsuarioAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

}

