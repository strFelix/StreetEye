using Microsoft.EntityFrameworkCore;
using StreetEye.data;
using StreetEye.models;

namespace StreetEye.Repository.Semaforos
{
    public sealed class SemaforoRepository : ISemaforoRepository
    {
        private readonly DataContext _context;
        public SemaforoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Semaforo> GetBySemaforoIdAsync(int id)
        {
            return await _context.Semaforos.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Semaforo>> GetAllAsync()
        {
            List<Semaforo> semaforos = await _context.Semaforos.ToListAsync();
            return semaforos;
        }

        public async Task AddStatusSemaforoAsync(StatusSemaforo statusSemaforo)
        {
            await _context.StatusSemaforos.AddAsync(statusSemaforo);
            await _context.SaveChangesAsync();
        }

        public async void UpdateSemaforo(Semaforo semaforo)
        {
            _context.Semaforos.Update(semaforo);
            await _context.SaveChangesAsync();
        }
    }
}
