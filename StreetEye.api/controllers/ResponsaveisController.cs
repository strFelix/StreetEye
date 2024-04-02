using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StreetEye.data;
using StreetEye.models;
using System.Security.Claims;

namespace StreetEye.api.controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ResponsaveisController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAcessor;

    public ResponsaveisController(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAcessor = httpContextAccessor;
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
            List<Utilizador> list = await _context.Utilizadores
                .Where(u => u.Tipo.Equals(models.enums.TipoUtilizador.RESPONSAVEL))
                .ToListAsync();

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
    public async Task<IActionResult> GetByUtilizadorIdAsync(int id)
    {
        try
        {
            Responsavel responsavel = await _context.Responsaveis
                .Include(r => r.ResponsavelUtilizador)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(u => u.IdResponsavel == id);

            if (responsavel == null)
                return NotFound();

            return Ok(responsavel.ResponsavelUtilizador);
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    #endregion

    #region Post
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Utilizador))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostUtilizadorAsync(Utilizador responsavelUtilizador)
    {
        try
        {
            responsavelUtilizador.Tipo = models.enums.TipoUtilizador.RESPONSAVEL;
            await _context.Utilizadores.AddAsync(responsavelUtilizador);
            await _context.SaveChangesAsync();

            int id = int.Parse(_httpContextAcessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Responsavel responsavel = new Responsavel
            {
                IdUtilizador = id,
                IdResponsavel = responsavelUtilizador.Id
            };

            await _context.Responsaveis.AddAsync(responsavel);
            await _context.SaveChangesAsync();

            return Created(nameof(ResponsaveisController), responsavelUtilizador);
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
    public async Task<IActionResult> UpdateResponsavelAsync(int id)
    {
        try
        {
            Utilizador responsavel = await _context.Utilizadores.FirstOrDefaultAsync(r => r.Id == id);

            if (responsavel == null)
                return NotFound();

            _context.Utilizadores.Update(responsavel);
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteResponsavelAsync(int id)
    {
        try
        {
            Utilizador responsavelUtilizador = await _context.Utilizadores.FirstOrDefaultAsync(r => r.Id == id);
            Responsavel responsavel = await _context.Responsaveis.FirstOrDefaultAsync(r => r.IdResponsavel == id);

            if (responsavelUtilizador == null || responsavel == null)
                return NotFound();

            _context.Responsaveis.Remove(responsavel);
            _context.Utilizadores.Remove(responsavelUtilizador);
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