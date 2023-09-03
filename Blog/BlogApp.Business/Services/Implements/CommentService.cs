using AutoMapper;
using BlogApp.Business.Dtos.CommentDtos;
using BlogApp.Business.Exceptions.Common;
using BlogApp.Business.Exceptions.UserExceptions;
using BlogApp.Business.Services.Interfaces;
using BlogApp.Core.Entities;
using BlogApp.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Business.Services.Implements;

public class CommentService : ICommentService
{
    readonly ICommentRepository _commentRepo;
    readonly IMapper _mapper;
    readonly UserManager<AppUser> _userManager;
    readonly string? _userId;
    readonly IBlogRepository _blogRepository;
    readonly IHttpContextAccessor _contextAccessor;

    public CommentService(ICommentRepository commentRepo, 
        IMapper mapper, UserManager<AppUser> userManager, 
         IBlogRepository blogRepository, 
        IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        _commentRepo = commentRepo;
        _mapper = mapper;
        _userManager = userManager;
        _userId = _contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _blogRepository = blogRepository;
    }

    public async Task CreateAsync(int id, CommentCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(_userId)) throw new ArgumentNullException();
        if (!await _userManager.Users.AnyAsync(u => u.Id == _userId)) throw new UserNotFoundException();
        if (id <= 0) throw new NegativeIdException();
        if (!await _blogRepository.IsExistAsync(b => b.Id == id)) throw new NotFoundException<Blog>();
        var comment = _mapper.Map<Comment>(dto);
        comment.AppUserId = _userId;
        comment.BlogId = id;
        await _commentRepo.CreateAsync(comment);
        await _commentRepo.SaveAsync();
    }

    public Task<IEnumerable<CommentListItemDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
