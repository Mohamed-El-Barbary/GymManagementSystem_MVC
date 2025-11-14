using GymManagementBLL.ViewModels.AdminManamentViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IAdminService
    {
        IEnumerable<AdminListViewModel> GetAllAdmins();
        IdentityResult CreateAdmin(CreateAdminViewModel model);
        IdentityResult EditAdmin(EditAdminViewModel model);
        IdentityResult DeleteAdmin(string id);
        IdentityResult ResetPassword(ResetPasswordViewModel model);
        ManageRolesViewModel GetRoles(string userId);
        IdentityResult AddRole(string userId, string role);
        IdentityResult RemoveRole(string userId, string role);
    }
}
