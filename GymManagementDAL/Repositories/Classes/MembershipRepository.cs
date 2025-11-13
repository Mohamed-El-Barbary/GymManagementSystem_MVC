using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Membership> GetAllMembershipsWithMemberAndPLan(Func<Membership, bool>? condition = null)
        {

            return _dbContext.Memberships
                             .Include(x => x.Plan)
                             .Include(x => x.Member)
                             .Where(condition)
                             .ToList();
        }
    }
}
