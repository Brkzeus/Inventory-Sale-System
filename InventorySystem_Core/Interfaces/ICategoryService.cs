using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem_Core.Interfaces
{
    public interface ICategoryService
    {
        Task<APIResponse<List<CategoryResponseDTO>>> GetAllAsync();
        Task<APIResponse<CategoryResponseDTO>> GetByIdAsync(int id);
        Task<APIResponse<CategoryResponseDTO>> CreateAsync(CreateCategoryDTO dto);
        Task<APIResponse<DummyClass>> UpdateAsync(int id, UpdateCategoryDTO dto);
        Task<APIResponse<DummyClass>> DeleteAsync(int id);
    }
}
