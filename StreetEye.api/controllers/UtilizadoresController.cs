using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StreetEye.data;
using StreetEye.models;

using PhoneNumbers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace StreetEye.api.controllers;

[ApiController]
[Route("[controller]")]
public class UtilizadoresController : ControllerBase
{
    private readonly DataContext _context;

    public UtilizadoresController (DataContext context){
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
    public async Task<IActionResult> GetUtilizadores(){
        try
        {
            List<Utilizador> list = await _context.Utilizadores.ToListAsync();
            return Ok(list);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUtilizador(int id){
        try
        {
            Utilizador search = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Id == id);
            return Ok(search);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
        
    #endregion

    #region Post
    [HttpPost]
    public async Task<IActionResult> InserirUtilizador(Utilizador utilizador){
        try
        {   
            // verificação data de nascimento (yyyy/MM/dd)
            DateTime dataNascimento = new DateTime(utilizador.DataNascimento.Year, utilizador.DataNascimento.Month, utilizador.DataNascimento.Day);
            if (ValidarDataNascimento(dataNascimento))  
                throw new Exception("Data de nascimento invalida.");

            //verificação numero de telefone (9xxxx-xxxx)
            if(ValidarNumeroTelefone(utilizador.Telefone))
                throw new Exception("Numero de telefone invalido.");

            // registrar latitude e longitude de acordo com endereço passado

            await _context.Utilizadores.AddAsync(utilizador);
            await _context.SaveChangesAsync();

            return Ok(utilizador);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    } 
    #endregion
    
    #region Put
    [HttpPut]
    public async Task<IActionResult> AtualizarUtilizador(Utilizador utilizador){
    try
        {       
            // alterar nome, telefone e endereco apenas
            // registrar latitude e longitude de acordo com endereço passado

            if(ValidarNumeroTelefone(utilizador.Telefone))
                throw new Exception("Numero de telefone invalido.");

            _context.Utilizadores.Update(utilizador);
            await _context.SaveChangesAsync();

            return Ok(utilizador);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
}