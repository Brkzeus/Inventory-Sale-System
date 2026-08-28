namespace InventorySystem_Services.Services;

using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.CategoryDTOs;
using InventorySystem_Core.Entities;
using InventorySystem_Core.Interfaces;
using InventorySystem_Infrastructure.Data;
using InventorySystem_Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse<List<CategoryResponseDTO>>> GetAllAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();

        var categoryDtos = categories.Select(c => new CategoryResponseDTO
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();

        return APIResponse<List<CategoryResponseDTO>>.SuccessResponse(categoryDtos, "Categories retrieved successfully.");
    }

    public async Task<APIResponse<CategoryResponseDTO>> GetByIdAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
        {
            return APIResponse<CategoryResponseDTO>.FailureResponse(
                "Category not found",
                new List<string> { $"Category with ID {id} was not found." }
            );
        }

        var dto = new CategoryResponseDTO
        {
            Id = category.Id,
            Name = category.Name
        };

        return APIResponse<CategoryResponseDTO>.SuccessResponse(dto, "Category retrieved successfully.");
    }

    public async Task<APIResponse<CategoryResponseDTO>> CreateAsync(CreateCategoryDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return APIResponse<CategoryResponseDTO>.FailureResponse(
                "Validation failed",
                new List<string> { "Category name cannot be empty." }
            );
        }

        var category = new Category
        {
            Name = dto.Name
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.CompleteAsync();

        var responseDto = new CategoryResponseDTO
        {
            Id = category.Id,
            Name = category.Name
        };

        return APIResponse<CategoryResponseDTO>.SuccessResponse(responseDto, "Category created successfully.");
    }

    public async Task<APIResponse<DummyClass>> UpdateAsync(int id, UpdateCategoryDTO dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
        {
            return APIResponse<DummyClass>.FailureResponse(
                "Update failed",
                new List<string> { $"Cannot update. Category with ID {id} was not found." }
            );
        }

        category.Name = dto.Name;

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.CompleteAsync();

        return APIResponse<DummyClass>.SuccessResponse(new DummyClass(), "Category updated successfully.");
    }

    public async Task<APIResponse<DummyClass>> DeleteAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
        {
            return APIResponse<DummyClass>.FailureResponse(
                "Delete failed",
                new List<string> { $"Cannot delete. Category with ID {id} was not found." }
            );
        }

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.CompleteAsync();

        return APIResponse<DummyClass>.SuccessResponse(new DummyClass(), "Category deleted successfully.");
    }
}