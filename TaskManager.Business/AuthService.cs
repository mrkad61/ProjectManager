using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess;
using TaskManager.Entities.DTOs;
using TaskManager.Core;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities;
using TaskManager.Entities.Helper;


namespace TaskManager.Business
{
    public class AuthService(
        IConfiguration config,
        IUserRepository userRepository,
        IUserService userService,
        IMapper mapper)
        : IAuthService
    {
        public async Task<string> RegisterAsync(UserRegisterDto dto)
        {
            await userRepository.IsEmailExist(dto.Email);

            var newUser = mapper.Map<User>(dto);
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            newUser.Role = UserType.Worker;
            
            await userRepository.AddAsync(newUser);
            
            return GenerateToken(newUser);
        }
        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var user = await userService.AuthenticateAsync(dto.Email, dto.Password);
            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>();
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.ExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
