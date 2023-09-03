using AutoMapper;
using BlogApp.Business.Dtos.BlogDtos;
using BlogApp.Business.Dtos.CategoryDtos;
using BlogApp.Business.Exceptions.Category;
using BlogApp.Business.Exceptions.Common;
using BlogApp.Business.Exceptions.UserExceptions;
using BlogApp.Business.Services.Interfaces;
using BlogApp.Core.Entities;
using BlogApp.Core.Enums;
using BlogApp.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Business.Services.Implements;

public class BlogService : IBlogService
{
    readonly IBlogRepository _repo;
    readonly IHttpContextAccessor _context;
    readonly IBlogLikeRepository _likeRepository;
    readonly IMapper _mapper;
    readonly string? userId;
    readonly ICategoryRepository _categoryRepository;
    readonly UserManager<AppUser> _userManager;

    public BlogService(IBlogRepository repo,
        IHttpContextAccessor context,
        IMapper mapper,
        ICategoryRepository categoryRepository,
        UserManager<AppUser> userManager,
        IBlogLikeRepository likeRepository)
    {
        _repo = repo;
        _context = context;
        //Login olmus userin id si
        userId = _context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
        _userManager = userManager;
        _likeRepository = likeRepository;
    }

    public async Task CreateAsync(BlogCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException();
        if (!await _userManager.Users.AnyAsync(u => u.Id == userId)) throw new UserNotFoundException();
        List<BlogCategory> blogCats = new();
        Blog blog = _mapper.Map<Blog>(dto);
        foreach (var id in dto.CategoryIds)
        {
            var cat = await _categoryRepository.FindByIdAsync(id);
            if (cat == null) throw new CategoryNotFoundException();
            blogCats.Add(new BlogCategory { Category = cat, Blog = blog });
        }
        blog.AppUserId = userId;
        blog.BlogCategories = blogCats;
        await _repo.CreateAsync(blog);
        await _repo.SaveAsync();
    }


    public async Task<IEnumerable<BlogListItemDto>> GetAllAsync()
    {
        var dto = new List<BlogListItemDto>();
        var entity = _repo.GetAll("AppUser", "BlogCategories", "BlogCategories.Category",
            "Comments", "Comments.Children", "Comments.AppUser","BlogLikes");
        List<Category> categories = new();
        foreach (var item in entity)
        {
            categories.Clear();
            foreach (var category in item.BlogCategories)
            {
                categories.Add(category.Category);
            }
            var dtoItem = _mapper.Map<BlogListItemDto>(item);
            dtoItem.Categories = _mapper.Map<IEnumerable<CategoryListItemDto>>(categories);
            dtoItem.ReactCount = item.BlogLikes.Count;
            dto.Add(dtoItem);
        }
        return dto;
        //var entity = _repo.GetAll("AppUser", "BlogCategories", "BlogCategories.Category");
        //return _mapper.Map<IEnumerable<BlogListItemDto>>(entity);
    }

    public async Task<BlogDetailDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException();
        var entity = await _repo.FindByIdAsync(id, "AppUser", "BlogCategories", "BlogCategories.Category",
            "Comments", "Comments.Children", "Comments.AppUser","BlogLikes", "BlogLikes.AppUser");
        if (entity == null) throw new NotFoundException<Blog>();
        entity.ViewerCount++;
        await _repo.SaveAsync();
        //var dto = _mapper.Map<BlogDetailDto>(entity);
        return _mapper.Map<BlogDetailDto>(entity);
    }

    public async Task ReactAsync(int id, Reactions reaction)
    {
        await _checkValidate(id);
        var blog = await _repo.FindByIdAsync(id, "BlogLikes");
        if (!blog.BlogLikes.Any(bl => blog.AppUserId == userId && bl.BlogId == id))
        {
            blog.BlogLikes.Add(new BlogLike { BlogId = id, AppUserId = userId, Reaction = reaction });
        }
        else
        {
            var currentReaction = blog.BlogLikes.
                FirstOrDefault(bl => blog.AppUserId == userId && bl.BlogId == id);
            if (currentReaction == null) throw new NotFoundException<BlogLike>();
            currentReaction.Reaction = reaction;
        }
        await _repo.SaveAsync();
    }

    public async Task RemoveAsync(int id)
    {
        await _checkValidate(id);
        var entity = await _repo.FindByIdAsync(id);
        if (entity == null) throw new NotFoundException<Blog>();
        if (entity.AppUserId != userId) throw new UserHasNoAccessException();
        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task RemoveReactAsync(int id)
    {
        await _checkValidate(id);
        var entity = await _likeRepository.GetSingleAsync(bl => bl.AppUserId == userId && bl.BlogId == id);
        if (entity == null) throw new NotFoundException<BlogLike>();
        _likeRepository.Delete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, BlogUpdateDto dto)
    {
        if (String.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException();
        if (!await _userManager.Users.AnyAsync(u => u.Id == userId)) throw new UserNotFoundException();
        var entity = await _repo.GetAll("BlogCategories", "BlogCategories.Category")
                                 .SingleOrDefaultAsync(blog => blog.Id == id);
        if (entity == null) throw new NotFoundException<Blog>();
        if (entity.AppUserId != userId) throw new UserHasNoAccessException();

        entity.BlogCategories?.Clear();

        foreach (var itemId in dto.CategoryIds)
        {
            var cat = await _categoryRepository.FindByIdAsync(itemId);
            if (cat == null) throw new CategoryNotFoundException();
            entity.BlogCategories?.Add(new BlogCategory { Category = cat });
        }

        entity.AppUserId = userId;
        _mapper.Map(dto, entity);

        await _repo.SaveAsync();
    }
    async Task _checkValidate(int id)
    {
        if (id <= 0) throw new NegativeIdException();
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException();
        if (!await _userManager.Users.AnyAsync(u => u.Id == userId)) throw new UserNotFoundException();
    }
}
