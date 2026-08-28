namespace InventorySystem_Services.Services;

using InventorySystem_Core.Constants;
using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.UserDTO;
using InventorySystem_Core.Entities;
using InventorySystem_Core.Interfaces;
using InventorySystem_Infrastructure.Data;
using InventorySystem_Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse<List<UserResponseDTO>>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();

        var userDtos = users.Select(u => new UserResponseDTO
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Role = u.Role
        }).ToList();

        return APIResponse<List<UserResponseDTO>>.SuccessResponse(userDtos, "Users retrieved successfully.");
    }

    public async Task<APIResponse<UserResponseDTO>> GetByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user == null)
        {
            return APIResponse<UserResponseDTO>.FailureResponse(
                "User not found",
                new List<string> { $"User with ID {id} was not found." }
            );
        }

        var dto = new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };

        return APIResponse<UserResponseDTO>.SuccessResponse(dto, "User retrieved successfully.");
    }

    public async Task<APIResponse<UserResponseDTO>> RegisterAsync(CreateUserDTO dto)
    {
        // 1. Check for duplicate email using specialized UserRepository method
        var emailExists = await _unitOfWork.Users.ExistsByEmailAsync(dto.Email);
        if (emailExists)
        {
            return APIResponse<UserResponseDTO>.FailureResponse(
                "Registration failed",
                new List<string> { "Email is already in use." }
            );
        }

        // 2. Map entity with password hashing
        var user = new User
        {
            Email = dto.Email,
            Username = dto.Username,
            passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = string.IsNullOrWhiteSpace(dto.Role) ? UserRoles.Customer : dto.Role
        };

        // 3. Persist via repository and commit through UnitOfWork
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CompleteAsync();

        var responseDto = new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };

        return APIResponse<UserResponseDTO>.SuccessResponse(responseDto, "User registered successfully.");
    }

    public async Task<APIResponse<DummyClass>> UpdateAsync(int id, UpdateUserDTO dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user == null)
        {
            return APIResponse<DummyClass>.FailureResponse(
                "Update failed",
                new List<string> { $"Cannot update. User with ID {id} was not found." }
            );
        }

        user.Username = dto.Username;
        user.Email = dto.Email;
        user.Role = dto.Role;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.CompleteAsync();

        return APIResponse<DummyClass>.SuccessResponse(new DummyClass(), "User updated successfully.");
    }

    public async Task<APIResponse<DummyClass>> DeleteAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user == null)
        {
            return APIResponse<DummyClass>.FailureResponse(
                "Delete failed",
                new List<string> { $"Cannot delete. User with ID {id} was not found." }
            );
        }

        _unitOfWork.Users.Delete(user);
        await _unitOfWork.CompleteAsync();

        return APIResponse<DummyClass>.SuccessResponse(new DummyClass(), "User deleted successfully.");
    }
}