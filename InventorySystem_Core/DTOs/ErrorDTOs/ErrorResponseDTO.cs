using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem_Core.DTOs.ErrorDTOs
{
    public class ErrorResponseDTO
    {
        public int StatusCode;
        public String Message;
        public String Details;
        public DateTime Timestamp = DateTime.UtcNow;
        
    }
}
