using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhoneNumbers;
using StreetEye.data;
using StreetEye.models;

namespace StreetEye.api.controllers;

[ApiController]
[Route("[controller]")]
public class UtilizadoresController : ControllerBase
{
    private readonly DataContext _context;

    public UtilizadoresController(DataContext context)
    {
        _context = context;
    }

    #region Validations
    // data nascimento
    static bool ValidarDataNascimento(DateTime dataNascimento)
    {
        // está dentro de um intervalo aceitável
        if (dataNascimento.Year < 1900 || dataNascimento.Year > DateTime.Now.Year)
            return true;

        // é válido para o mês específico
        if (dataNascimento.Day < 1 || dataNascimento.Day > DateTime.DaysInMonth(dataNascimento.Year, dataNascimento.Month))
            return true;

        return false;
    }

    // numero de telefone
    static bool ValidarNumeroTelefone(string numeroTelefone)
    {
        PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            PhoneNumber number = phoneNumberUtil.Parse(numeroTelefone, "BR"); // "BR" é o código do país para o Brasil
            bool isValid = phoneNumberUtil.IsValidNumber(number);

            if (isValid)
                return true;

            return false;
        }
        catch (NumberParseException ex)
        {
            throw new Exception("Erro ao analisar o número de telefone: \n" + ex.Message);
        }
    }
    #endregion

    #region Get
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            List<Utilizador> list = await _context.Utilizadores.ToListAsync();

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
            Utilizador utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Id == id);

            if (utilizador == null)
                return NotFound();

            return Ok(utilizador);
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
    public async Task<IActionResult> PostUtilizadorAsync(Utilizador utilizador)
    {
        try
        {
            // verificação data de nascimento (yyyy/MM/dd)
            DateTime dataNascimento = new DateTime(utilizador.DataNascimento.Year, utilizador.DataNascimento.Month, utilizador.DataNascimento.Day);
            if (ValidarDataNascimento(dataNascimento))
                throw new Exception("Data de nascimento invalida.");

            //verificação numero de telefone (9xxxx-xxxx)
            if (ValidarNumeroTelefone(utilizador.Telefone))
                throw new Exception("Numero de telefone invalido.");

            // registrar latitude e longitude de acordo com endereço passado

            await _context.Utilizadores.AddAsync(utilizador);
            await _context.SaveChangesAsync();

            return Created(nameof(UtilizadoresController), utilizador);
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    #endregion

    #region Put
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutUtilizadorAsync(int id)
    {
        try
        {
            // alterar nome, telefone e endereco apenas
            // registrar latitude e longitude de acordo com endereço passado

            Utilizador utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Id == id);

            if (utilizador == null)
                return NotFound();

            if (ValidarNumeroTelefone(utilizador.Telefone))
                throw new Exception("Numero de telefone invalido.");

            _context.Utilizadores.Update(utilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    #endregion
}