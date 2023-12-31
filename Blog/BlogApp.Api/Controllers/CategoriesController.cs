﻿using BlogApp.Business.Dtos.CategoryDtos;
using BlogApp.Business.Exceptions.Category;
using BlogApp.Business.Exceptions.Common;
using BlogApp.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.GetAllAsync());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromForm] CategoryCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return NoContent();
    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Put(int id, [FromForm] CategoryUpdateDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoveAsync(id);
        return NoContent();
    }
}
