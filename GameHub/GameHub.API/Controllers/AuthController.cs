using GameHub.API.Dtos.Auth;
using GameHub.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.JSInterop.Infrastructure;

namespace GameHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    //Construtor que cuida do acesso
    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        var userExists = await _userManager.FindByNameAsync(dto.UserName);

        if (userExists != null)
        {
            return BadRequest("Usuario ja Existe");
        }

        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, dto.Password); // cria usuario novo

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Usuario criado com Sucesso");

    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.Username); // busca usuario

        if (user is null)
        {
            return Unauthorized("Usuario ou senha Invalidos");
        }

        //compara senhas digitada com banco de dados
        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            dto.Password,
            false);

        if(!result.Succeeded)
        {
            return Unauthorized("Usuario ou se");
        }

        return Ok("Login realizado com sucesso");
    }
}
