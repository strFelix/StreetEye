using StreetEye.models;

namespace StreetEye.Repository.Semaforos
{
    public interface ISemaforoRepository
    {
        public Task<Semaforo> GetBySemaforoIdAsync(int id);
        public Task<List<Semaforo>> GetAllAsync();
        public Task AddStatusSemaforoAsync(StatusSemaforo statusSemaforo);
        public void UpdateSemaforo(Semaforo semaforo);
    }
}
