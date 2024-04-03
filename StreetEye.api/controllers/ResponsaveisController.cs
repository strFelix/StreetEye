using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StreetEye.models;
using StreetEye.Repository.Responsaveis;
using StreetEye.Repository.Utilizadores;
using System.Security.Claims;

namespace StreetEye.api.controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ResponsaveisController : ControllerBase
{
    private readonly IResponsavelRepository _responsavelRepository;
    private readonly IUtilizadorRepository _utilizadorRepository;
    private readonly IHttpContextAccessor _httpContextAcessor;

    public ResponsaveisController(
        IResponsavelRepository responsavelRepository, 
        IUtilizadorRepository utilizadorRepository, 
        IHttpContextAccessor httpContextAccessor)
    {
        _responsavelRepository = responsavelRepository;
        _utilizadorRepository = utilizadorRepository;
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
            List<Utilizador> list = await _utilizadorRepository.GetAllResponsaveisAsync();

            if (list.IsNullOrEmpty())
                return NoContent();

            return Ok(list);
        }
        catch (Exception ex)
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
            Responsavel responsavel = await _responsavelRepository.GetByUtilizadorIdAsync(id);

            if (responsavel == null)
                return NotFound();

            return Ok(responsavel.ResponsavelUtilizador);
        }
        catch (Exception ex)
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
            _utilizadorRepository.AddUtilizadorAsync(responsavelUtilizador);

            int id = int.Parse(_httpContextAcessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Responsavel responsavel = new Responsavel
            {
                IdUtilizador = id,
                IdResponsavel = responsavelUtilizador.Id
            };

            _responsavelRepository.AddResponsavelAsync(responsavel);

            return Created(nameof(ResponsaveisController), responsavelUtilizador);
        }
        catch (Exception ex)
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
            Utilizador responsavel = await _utilizadorRepository.GetUtilizadorAsync(id);
            if (responsavel == null)
                return NotFound();

            _utilizadorRepository.UpdateUtilizadorAsync(responsavel);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    #endregion

    #region Delete
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteResponsavelAsync(int id)
    {
        try
        {
            Utilizador responsavelUtilizador = await _utilizadorRepository.GetUtilizadorAsync(id);
            Responsavel responsavel = await _responsavelRepository.GetResponsavelAsync(id);

            if (responsavelUtilizador == null || responsavel == null)
                return NotFound();

            _responsavelRepository.DeleteResponsavelAsync(responsavel);
            _utilizadorRepository.DeleteUtilizadorAsync(responsavelUtilizador);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    #endregion
}