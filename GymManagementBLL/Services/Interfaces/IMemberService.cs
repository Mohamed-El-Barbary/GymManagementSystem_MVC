using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {

        IEnumerable<MemberViewModel> GetAllMembers();
        HealthRecordViewModel? GetMemberHealthRecord(int id);

        MemberToUpdateViewModel? GetMemberToUpdate(int id);

        MemberViewModel? GetMemberDetails(int id);

        bool CreateMember(CreateMemberViewModel createdMember);

        bool UpdateMember(int id, MemberToUpdateViewModel updatedMember);

        bool RemoveMember(int id);

    }
}
