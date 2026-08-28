using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem_Core.Interfaces
{
    public interface IUserService
    {
        Task<APIResponse<List<UserResponseDTO>>> GetAllAsync();
        Task<APIResponse<UserResponseDTO>> GetByIdAsync(int id);
        Task<APIResponse<UserResponseDTO>> RegisterAsync(CreateUserDTO dto);
        Task<APIResponse<DummyClass>> UpdateAsync(int id, UpdateUserDTO dto);
        Task<APIResponse<DummyClass>> DeleteAsync(int id);
    }
}
