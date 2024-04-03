using StreetEye.models;

namespace StreetEye.Repository.Responsaveis;

public interface IResponsavelRepository
{
    public Task<Responsavel> GetResponsavelAsync(int id);
    public Task<Responsavel> GetByUtilizadorIdAsync(int id);
    public void AddResponsavelAsync(Responsavel responsavel);
    public void DeleteResponsavelAsync(Responsavel responsavel);
}

