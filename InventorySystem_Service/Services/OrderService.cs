namespace InventorySystem_Services.Services;

using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.OrderDTOs;
using InventorySystem_Core.Entities;
using InventorySystem_Core.Interfaces;
using InventorySystem_Infrastructure.Data;
using InventorySystem_Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse<List<OrderResponseDTO>>> GetAllAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();

        var orderDtos = orders.Select(o => new OrderResponseDTO
        {
            Id = o.Id,
            UserId = o.UserId,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Items = o.OrderItems.Select(oi => new OrderItemResponseDTO
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name ?? "Unknown",
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList()
        }).ToList();

        return APIResponse<List<OrderResponseDTO>>.SuccessResponse(orderDtos, "Orders retrieved successfully.");
    }

    public async Task<APIResponse<OrderResponseDTO>> GetByIdAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);

        if (order == null)
        {
            return APIResponse<OrderResponseDTO>.FailureResponse(
                "Order not found",
                new List<string> { $"Order with ID {id} was not found." }
            );
        }

        var dto = new OrderResponseDTO
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(oi => new OrderItemResponseDTO
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name ?? "Unknown",
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList()
        };

        return APIResponse<OrderResponseDTO>.SuccessResponse(dto, "Order retrieved successfully.");
    }

    public async Task<APIResponse<OrderResponseDTO>> CreateAsync(CreateOrderDTO dto)
    {
        if (dto.Items == null || !dto.Items.Any())
        {
            return APIResponse<OrderResponseDTO>.FailureResponse(
                "Order creation failed",
                new List<string> { "An order must contain at least one item." }
            );
        }

        var userExists = await _unitOfWork.Users.GetByIdAsync(dto.UserId);
        if (userExists == null)
        {
            return APIResponse<OrderResponseDTO>.FailureResponse(
                "Order creation failed",
                new List<string> { $"User with ID {dto.UserId} does not exist." }
            );
        }

        var order = new Order
        {
            UserId = dto.UserId,
            OrderDate = DateTime.UtcNow,
            OrderItems = new List<OrderItem>()
        };

        decimal calculatedTotal = 0;
        var createdItems = new List<OrderItemResponseDTO>();

        foreach (var itemDto in dto.Items)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);

            if (product == null)
            {
                return APIResponse<OrderResponseDTO>.FailureResponse(
                    "Order creation failed",
                    new List<string> { $"Product with ID {itemDto.ProductId} was not found." }
                );
            }

            if (product.StockQuantity < itemDto.Quantity)
            {
                return APIResponse<OrderResponseDTO>.FailureResponse(
                    "Order creation failed",
                    new List<string> { $"Insufficient stock for product '{product.Name}'. Available: {product.StockQuantity}" }
                );
            }

            // Deduct stock on tracked entity
            product.StockQuantity -= itemDto.Quantity;
            _unitOfWork.Products.Update(product);

            var orderItem = new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            };

            calculatedTotal += orderItem.UnitPrice * orderItem.Quantity;
            order.OrderItems.Add(orderItem);

            createdItems.Add(new OrderItemResponseDTO
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalAmount = calculatedTotal;

        await _unitOfWork.Orders.AddAsync(order);

        // Commits both the new Order/OrderItems AND updated product stock counts atomically
        await _unitOfWork.CompleteAsync();

        var responseDto = new OrderResponseDTO
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Items = createdItems
        };

        return APIResponse<OrderResponseDTO>.SuccessResponse(responseDto, "Order created successfully.");
    }

    public async Task<APIResponse<DummyClass>> DeleteAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);

        if (order == null)
        {
            return APIResponse<DummyClass>.FailureResponse(
                "Delete failed",
                new List<string> { $"Cannot delete. Order with ID {id} was not found." }
            );
        }

        _unitOfWork.Orders.Delete(order);
        await _unitOfWork.CompleteAsync();

        return APIResponse<DummyClass>.SuccessResponse(new DummyClass(), "Order deleted successfully.");
    }
}