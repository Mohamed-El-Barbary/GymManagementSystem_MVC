using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.AdminManamentViewModels
{
    public class ManageRolesViewModel
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;

        public List<string> UserRoles { get; set; } = new List<string>();
        public List<string> AllRoles { get; set; } = new List<string>();
    }
}
