using InventorySystem_Core.DTOs;
using InventorySystem_Core.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InventorySystem_Core.Interfaces
{
    public interface IAuthService
    {
        Task<APIResponse<DummyClass>> RegisterAsync(RegisterDTO dto);
        Task<APIResponse<AuthResponseDTO>> LoginAsync(LoginDTO dto);
    }
}

