using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StreetEye.data;
using StreetEye.models;

namespace StreetEye.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SemaforosController : ControllerBase
    {
        private readonly DataContext _context;

        public SemaforosController(DataContext context)
        {
            _context = context;
        }

        #region Get

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Semaforo> list = await _context.Semaforos.ToListAsync();

                if (list.IsNullOrEmpty())
                    return NoContent();

                return Ok(list);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBySemaforoIdAsync(int id)
        {
            try
            {
                Semaforo semaforo = await _context.Semaforos.FirstOrDefaultAsync(s => s.Id == id);

                if (semaforo == null)
                    return NotFound("Semaforo " + id + " não enocontrado");

                return Ok(semaforo);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

        #region Post

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Semaforo))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostSemaforoAsync(Semaforo semaforo)
        {
            try
            {
                await _context.Semaforos.AddAsync(semaforo);
                await _context.SaveChangesAsync();

                return Created(nameof(SemaforosController), semaforo);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

        #region Put

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutSemaforoAsync(int id)
        {
            try
            {
                Semaforo semaforo = await _context.Semaforos.FirstOrDefaultAsync(s => s.Id == id);

                if (semaforo == null)
                    return NotFound("Semaforo" + id + "não encontrado");

                _context.Semaforos.Update(semaforo);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSemaforoAsync(int id)
        {
            try
            {
                Semaforo semaforo = await _context.Semaforos.FirstOrDefaultAsync(s => s.Id == id);

                if (semaforo == null)
                    return NotFound("Semaforo" + id + "não encontrado");

                _context.Semaforos.Remove(semaforo);
                int rows = await _context.SaveChangesAsync();
                return Ok(rows);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}