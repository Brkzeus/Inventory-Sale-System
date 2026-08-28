using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem_Core.Constants
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        // Utility list for validation (e.g., checking if a requested role exists)
        public static readonly IReadOnlyList<string> AllRoles = new List<string>
    {
        Admin,
        Customer
    };
    }
}
