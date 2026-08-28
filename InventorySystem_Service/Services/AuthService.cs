using Azure.Core;
using InventorySystem_Core.Constants;
using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.AuthDTOs;
using InventorySystem_Core.Entities;
using InventorySystem_Core.Exceptions;
using InventorySystem_Core.Interfaces;
using InventorySystem_Infrastructure.Data;
using InventorySystem_Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventorySystem_Services.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<APIResponse<DummyClass>> RegisterAsync(RegisterDTO dto)
    {
        var userExists = await _unitOfWork.Users.ExistsByEmailAsync(dto.Email);
        if (userExists)
        {
            return APIResponse<DummyClass>.FailureResponse(
                "Registration failed",
                new List<string> { "Email is already registered." }
            );
        }

        var newUser = new User
        {
            Email = dto.Email,
            Username = dto.Username,
            passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = string.IsNullOrWhiteSpace(dto.Role) ? UserRoles.Customer : dto.Role,
            CreatedAt = DateTime.UtcNow // MUST be explicitly set
        };

        await _unitOfWork.Users.AddAsync(newUser);
        await _unitOfWork.CompleteAsync();

        return APIResponse<DummyClass>.SuccessResponse(new DummyClass(), "User registered successfully.");
    }

    public async Task<APIResponse<AuthResponseDTO>> LoginAsync(LoginDTO dto)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            return APIResponse<AuthResponseDTO>.FailureResponse(
                "Login failed",
                new List<string> { "Invalid email or password." }
            );
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.passwordHash);
        if (!isPasswordValid)
        {
            return APIResponse<AuthResponseDTO>.FailureResponse("Login failed",new List<string> { "Invalid email or password." });
        }

        string token = GenerateJwtToken(user);

        var responseDto = new AuthResponseDTO
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = token
        };

        return APIResponse<AuthResponseDTO>.SuccessResponse(responseDto, "Login successful.");
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(secretKey);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
