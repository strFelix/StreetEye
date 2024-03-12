using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreetEye.data;
using StreetEye.models;

namespace StreetEye.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SemaforosController : ControllerBase
    {
        private readonly DataContext _context;

        public SemaforosController (DataContext context){
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> GetSemaforos(){
            try
            {
                List<Semaforo> list = await _context.Semaforos.ToListAsync();
                return Ok(list);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSingle/{id}")]
        public async Task<IActionResult> GetSemaforo(int id){
            try
            {
                Semaforo semaforo = await _context.Semaforos.FirstOrDefaultAsync(s => s.Id == id);
                return Ok(semaforo);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // POST
        [HttpPost]
        public async Task<IActionResult> PostSemaforo(Semaforo semaforo){
            try
            {
                await _context.Semaforos.AddAsync(semaforo);
                await _context.SaveChangesAsync();

                return Ok(semaforo);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT
        [HttpPut]
        public async Task<IActionResult> PutSemaforo(Semaforo semaforo){
            try
            {
                _context.Semaforos.Update(semaforo);
                int rows = await _context.SaveChangesAsync();
                return Ok(rows);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemaforo(int id){
            try
            {
                Semaforo semaforo = await _context.Semaforos.FirstOrDefaultAsync(s => s.Id == id);
                _context.Semaforos.Remove(semaforo);
                int rows = await _context.SaveChangesAsync();
                return Ok(rows);
            }
            catch (System.Exception ex)
            {   
                return BadRequest(ex.Message);
            }
        }
    }
}