using StreetEye.models;

namespace StreetEye.Repository.Semaforos
{
    public interface ISemaforoRepository
    {
        public Task<Semaforo> GetBySemaforoIdAsync(int id);
        public void UpdateSemaforo(Semaforo semaforo);
    }
}
