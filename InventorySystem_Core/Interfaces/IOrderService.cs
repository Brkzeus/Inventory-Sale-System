using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InventorySystem_Core.Interfaces
{
    
    public interface IOrderService
    {

        Task<APIResponse<List<OrderResponseDTO>>> GetAllAsync();
        Task<APIResponse<OrderResponseDTO>> GetByIdAsync(int id);
        Task<APIResponse<OrderResponseDTO>> CreateAsync(CreateOrderDTO dto);
        Task<APIResponse<DummyClass>> DeleteAsync(int id);

    }
}
