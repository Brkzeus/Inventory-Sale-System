using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem_Core.Interfaces
{
    public interface IProductService
    {
        Task<APIResponse<List<ProductResponseDTO>>> GetAllAsync();
        Task<APIResponse<ProductResponseDTO>> GetByIdAsync(int id);
        Task<APIResponse<ProductResponseDTO>> CreateAsync(CreateProductDTO dto);
        Task<APIResponse<DummyClass>> UpdateAsync(int id, UpdateProductDTO dto);
        Task<APIResponse<DummyClass>> DeleteAsync(int id);
    }
}
