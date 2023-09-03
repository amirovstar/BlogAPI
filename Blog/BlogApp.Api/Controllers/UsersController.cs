using BlogApp.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _userService.GetAllAsync());
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("[action]")]
    public async Task<IActionResult> AddRole(string role, string userName)
    {
        await _userService.AddRole(role, userName);
        return Ok();
    }
}
