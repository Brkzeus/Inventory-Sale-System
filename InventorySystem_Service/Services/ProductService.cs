namespace InventorySystem_Services.Services;

using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.ProductDTOs;
using InventorySystem_Core.Entities;
using InventorySystem_Core.Exceptions;
using InventorySystem_Core.Interfaces;
using InventorySystem_Infrastructure.Data;
using InventorySystem_Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse<List<ProductResponseDTO>>> GetAllAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();

        List<ProductResponseDTO> productDtos = products.Select(p => new ProductResponseDTO
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            CategoryId = p.CategoryId
        }).ToList();

        return APIResponse<List<ProductResponseDTO>>.SuccessResponse(productDtos, "Products retrieved successfully.");
    }

    public async Task<APIResponse<ProductResponseDTO>> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null)
        {
            return APIResponse<ProductResponseDTO>.FailureResponse(
                "Product not found",
                new List<string> { $"Product with ID {id} was not found." }
            );
        }

        var dto = new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId
        };

        return APIResponse<ProductResponseDTO>.SuccessResponse(dto, "Product retrieved successfully.");
    }

    public async Task<APIResponse<ProductResponseDTO>> CreateAsync(CreateProductDTO dto)
    {
        if (dto.price <= 0)
        {
            return APIResponse<ProductResponseDTO>.FailureResponse(
                "Validation failed",
                new List<string> { "Product price must be greater than zero." }
            );
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.CompleteAsync();

        var responseDto = new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId
        };

        return APIResponse<ProductResponseDTO>.SuccessResponse(responseDto, "Product created successfully.");
    }

    public async Task<APIResponse<DummyClass>> UpdateAsync(int id, UpdateProductDTO dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null)
        {
            return APIResponse<DummyClass>.FailureResponse(
                "Update failed",
                new List<string> { $"Cannot update. Product with ID {id} was not found." }
            );
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.CompleteAsync();

        return APIResponse<DummyClass>.SuccessResponse(new DummyClass(), "Product updated successfully.");
    }

    public async Task<APIResponse<DummyClass>> DeleteAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null)
        {
            return APIResponse<DummyClass>.FailureResponse(
                "Delete failed",
                new List<string> { $"Cannot delete. Product with ID {id} was not found." }
            );
        }

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.CompleteAsync();

        return APIResponse<DummyClass>.SuccessResponse(new DummyClass(), "Product deleted successfully.");
    }
}