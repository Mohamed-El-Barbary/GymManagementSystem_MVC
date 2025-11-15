using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        IEnumerable<Membership> GetAllMembershipsWithMemberAndPLan(Func<Membership, bool>? condition = null);
    }
}
