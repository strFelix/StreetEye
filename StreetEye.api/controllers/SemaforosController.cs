using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StreetEye.data;
using StreetEye.models;
using StreetEye.Repository.Semaforos;

namespace StreetEye.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SemaforosController : ControllerBase
    {
        private readonly ISemaforoRepository _SemaforoRepository;
        public SemaforosController(ISemaforoRepository semaforoRepository)
        {
            _SemaforoRepository = semaforoRepository;
        }

        #region Get

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBySemaforoIdAsync(int id)
        {
            try
            {
                Semaforo semaforo = await _SemaforoRepository.GetBySemaforoIdAsync(id);

                if (semaforo == null)
                    return NotFound("Semaforo não encontrado");

                return Ok(semaforo);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int id)
        {
            try
            {
                return Ok(await _SemaforoRepository.GetAllAsync());
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
                Semaforo semaforo = await _SemaforoRepository.GetBySemaforoIdAsync(id);

                if (semaforo == null)
                    return NotFound("Semaforo não encontrado");

                _SemaforoRepository.UpdateSemaforo(semaforo);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

    }
}