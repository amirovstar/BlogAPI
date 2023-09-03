using BlogApp.Business.Dtos.BlogDtos;
using BlogApp.Business.Dtos.CategoryDtos;
using BlogApp.Business.Dtos.CommentDtos;
using BlogApp.Business.Services.Interfaces;
using BlogApp.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    readonly IBlogService _blogService;
    readonly ICommentService _commissionService;

    public BlogsController(IBlogService blogService, ICommentService commissionService)
    {
        _blogService = blogService;
        _commissionService = commissionService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _blogService.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _blogService.GetByIdAsync(id));
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post(BlogCreateDto dto)
    {
        try
        {
            await _blogService.CreateAsync(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _blogService.RemoveAsync(id);
        return StatusCode(StatusCodes.Status202Accepted);
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] BlogUpdateDto dto)
    {
        await _blogService.UpdateAsync(id,dto);
        return StatusCode(StatusCodes.Status202Accepted);
    }
    [Authorize]
    [HttpPost("[action]/{id}")]
    public async Task<IActionResult> Comment(int id,CommentCreateDto dto)
    {
        await _commissionService.CreateAsync(id,dto);
        return StatusCode(StatusCodes.Status202Accepted);
    }
    [Authorize]
    [HttpPost("[action]/{id}")]
    public async Task<IActionResult> React(int id, Reactions reaction)
    {
        await _blogService.ReactAsync(id, reaction);
        return StatusCode(StatusCodes.Status202Accepted);
    }
    [Authorize]
    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> RemoveReact(int id)
    {
        await _blogService.RemoveReactAsync(id);
        return StatusCode(StatusCodes.Status202Accepted);
    }
}
