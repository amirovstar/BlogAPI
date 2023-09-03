using BlogApp.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _roleService.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await _roleService.GetByIdAsync(id));
    }
    [HttpPost]
    public async Task<IActionResult> Post(string name)
    {
        await _roleService.CreateAsync(name);
        return StatusCode(StatusCodes.Status201Created);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        await _roleService.RemoveAsync(id);
        return StatusCode(StatusCodes.Status204NoContent);
    }
    [HttpPut]
    public async Task<IActionResult> Put(string id,string name)
    {
        await _roleService.UpdateAsync(id,name);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}
