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

        public async void UpdateSemaforo(Semaforo semaforo)
        {
            _context.Semaforos.Update(semaforo);
            await _context.SaveChangesAsync();
        }
    }
}
