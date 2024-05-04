using StreetEye.Models;

namespace StreetEye.Repository.Responsaveis;

public interface IResponsavelRepository
{
    public Task<Responsavel> GetResponsavelAsync(int id);
    public Task<Responsavel> GetByUtilizadorIdAsync(int id);
    public Task AddResponsavelAsync(Responsavel responsavel);
    public void DeleteResponsavelAsync(Responsavel responsavel);
}

