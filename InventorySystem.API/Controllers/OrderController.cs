namespace InventorySystem.API.Controllers;

using InventorySystem_Core.Constants;
using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.OrderDTOs;
using InventorySystem_Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _orderService.GetAllAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _orderService.GetByIdAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Customer},{UserRoles.Admin}")]
    public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
    {
        var response = await _orderService.CreateAsync(dto);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _orderService.DeleteAsync(id);
        if (!response.Success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
}