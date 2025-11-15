using GymManagementBLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMembershipService
    {

        IEnumerable<MembershipViewModel> GetAllMemberships();

        bool CreateMembership(CreateMembershipViewModel createdMembership);

        bool DeleteMembership(int id);

        IEnumerable<MemberSelectListViewModel> GetMembersForDropDown();
        IEnumerable<PlanSelectListViewModel> GetPlansForDropDown();

    }
}
