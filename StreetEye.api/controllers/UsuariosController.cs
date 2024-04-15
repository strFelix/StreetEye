using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RpgApi.Utils;
using StreetEye.data;
using StreetEye.models;
using StreetEye.Repository.Usuarios;
using StreetEye.Repository.Utilizadores;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace StreetEye.api.controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUtilizadorRepository _utilizadorRepository;

    public UsuariosController(IConfiguration configuration,
        IUsuarioRepository usuarioRepository,
        IUtilizadorRepository utilizadorRepository)
    {
        _configuration = configuration;
        _usuarioRepository = usuarioRepository;
        _utilizadorRepository = utilizadorRepository;
    }

    #region Token
    [AllowAnonymous]
    private string CreateToken(Usuario usuario)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUtilizador.ToString()),
                new Claim(ClaimTypes.Name, usuario.Utilizador.Nome)
            };
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("ConfiguracaoToken:Chave").Value));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddHours(4),
            SigningCredentials = creds
        };
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    #endregion


    #region Get

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            List<Usuario> usuarios = await _usuarioRepository.GetAllAsync();
            return Ok(usuarios);
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
    public async Task<IActionResult> GetByUsuarioIdAsync(int id)
    {
        try
        {
            Usuario usuario = await _usuarioRepository.GetByUsuarioIdAsync(id);

            if (usuario == null)
                return NotFound("Usuario " + id + "não entcontrado");

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    #endregion

    #region Post

    [AllowAnonymous]
    [HttpPost("Registrar")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Usuario))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegistrarUsuarioAsync(Usuario usuario)
    {
        try
        {
            Usuario usuarioExists = await _usuarioRepository.GetByUsuarioEmailAsync(usuario.Email);
            if (usuarioExists != null && !usuarioExists.Equals(usuario))
                return Conflict("Usuario já existe.");

            Criptografia.CriarPasswordHash(usuario.Password, out byte[] hash, out byte[] salt);

            usuario.Password = string.Empty;
            usuario.PasswordHash = hash;
            usuario.PasswordSalt = salt;
            
            await _usuarioRepository.AddUsuarioAsync(usuario);
            await _utilizadorRepository.AddUtilizadorAsync(usuario.Utilizador);

            return Created(nameof(UsuariosController), usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    [AllowAnonymous]
    [HttpPost("Autenticar")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AutenticarUsuarioAsync(Usuario credenciais)
    {
        try
        {
            Usuario? usuario = await _usuarioRepository.GetByUsuarioEmailAsync(credenciais.Email);

            if (usuario == null)
                return NotFound("Usuario não encontrado.");
            else if (!Criptografia.VerificarPasswordHash(credenciais.Password, usuario.PasswordHash, usuario.PasswordSalt))
                return BadRequest("Senha incorreta.");
            else
            {
                usuario.Utilizador = await _utilizadorRepository.GetUtilizadorAsync(usuario.IdUtilizador);

                usuario.PasswordHash = null;
                usuario.PasswordSalt = null;
                usuario.Token = CreateToken(usuario);
                return Ok(usuario);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    #endregion

    #region Put

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUsuarioAsync(int id)
    {
        try
        {
            Usuario usuario = await _usuarioRepository.GetByUsuarioIdAsync(id);

            if (usuario == null)
                return NotFound("Usuario não entcontrado");

            _usuarioRepository.UpdateUsuarioAsync(usuario);
            return NoContent();

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    #endregion
}