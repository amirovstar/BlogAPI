using AutoMapper;
using BlogApp.Business.Dtos.UserDtos;
using BlogApp.Business.Exceptions.Common;
using BlogApp.Business.Exceptions.Role;
using BlogApp.Business.Exceptions.UserExceptions;
using BlogApp.Business.ExternalServices.Interfaces;
using BlogApp.Business.Services.Interfaces;
using BlogApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BlogApp.Business.Services.Implements;

public class UserService : IUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly IMapper _mapper;
    readonly ITokenService _tokenService;

    public UserService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }

    public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user == null) throw new LoginFailedException("Username or Password is wromg");
        var result = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!result) throw new LoginFailedException("Username or Password is wromg");


        return _tokenService.CreateToken(user);
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        var user = _mapper.Map<AppUser>(dto);
        if (await _userManager.Users.AnyAsync(u => dto.UserName == u.UserName || dto.Email == u.Email))
        {
            throw new UserExistException();
        }
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in result.Errors)
            {
                sb.Append(item.Description + " ");
            }
            throw new RegisterFailedException(sb.ToString().TrimEnd());
        }
        var res = await _userManager.AddToRoleAsync(user, "Member");
    }
    public async Task AddRole(string roleName, string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) throw new NotFoundException<AppUser>();
        if (!await _roleManager.RoleExistsAsync(roleName)) throw new NotFoundException<IdentityRole>();
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            string a = "";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new RoleCreateFailedException(a);
        }
    }

    public Task RemoveRole(string roleName, string userName)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<UserWithRolesDto>> GetAllAsync()
    {
        ICollection<UserWithRolesDto> users = new List<UserWithRolesDto>();
        foreach (var item in await _userManager.Users.ToListAsync())
        {
            users.Add(new UserWithRolesDto
            {
                User = _mapper.Map<AuthorDto>(item),
                Roles = await _userManager.GetRolesAsync(item)
            });
        }
        return users;   
    }
}
