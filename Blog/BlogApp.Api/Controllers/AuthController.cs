using BlogApp.Business.Dtos.UserDtos;
using BlogApp.Business.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly IUserService _userService;
    readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(IUserService userService, RoleManager<IdentityRole> roleManager)
    {
        _userService = userService;
        _roleManager = roleManager;
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        await _userService.RegisterAsync(dto);
        return NoContent();
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        return Ok(await _userService.LoginAsync(dto));
    }
}
