using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhoneNumbers;
using StreetEye.models;
using StreetEye.Repository.Utilizadores;

namespace StreetEye.api.controllers;

[ApiController]
[Route("[controller]")]
public class UtilizadoresController : ControllerBase
{
    private readonly IUtilizadorRepository _utilizadorRepository;

    public UtilizadoresController(IUtilizadorRepository utilizadorRepository)
    {
        _utilizadorRepository = utilizadorRepository;
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
            List<Utilizador> list = await _utilizadorRepository.GetAllUtilizadoresAsync();

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
            Utilizador utilizador = await _utilizadorRepository.GetUtilizadorAsync(id);

            if (utilizador == null)
                return NotFound();

            return Ok(utilizador);
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

            _utilizadorRepository.AddUtilizadorAsync(utilizador);

            return Created(nameof(UtilizadoresController), utilizador);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    #endregion

    #region Put
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutUtilizadorAsync(Utilizador utilizadorUpdated)
    {
        try
        {
            // alterar nome, telefone e endereco apenas
            // registrar latitude e longitude de acordo com endereço passado

            Utilizador utilizador = await _utilizadorRepository.GetUtilizadorAsync(utilizadorUpdated.Id);

            if (utilizador == null)
                return NotFound();

            utilizador.Nome = utilizadorUpdated.Nome;
            utilizador.Telefone = utilizadorUpdated.Telefone;
            utilizador.CEP = utilizadorUpdated.CEP;
            utilizador.Endereco = utilizadorUpdated.Endereco;
            utilizador.NumeroEndereco = utilizadorUpdated.NumeroEndereco;
            utilizador.Complemento = utilizadorUpdated.Complemento;
            utilizador.Cidade = utilizadorUpdated.Cidade;
            utilizador.UF = utilizadorUpdated.UF;

            if (ValidarNumeroTelefone(utilizador.Telefone))
                throw new Exception("Numero de telefone sinvalido.");

            _utilizadorRepository.UpdateUtilizadorAsync(utilizador);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    #endregion
}