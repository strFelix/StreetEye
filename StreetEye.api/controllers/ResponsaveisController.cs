using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreetEye.data;
using StreetEye.models;

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
}
