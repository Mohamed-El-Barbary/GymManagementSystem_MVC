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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory(Func<Session, bool>? condition = null)
        {

            if (condition is null)
                return _dbContext.Sessions.Include(X => X.SessionTrainer)
                    .Include(X => X.SessionCategory)
                    .ToList();
            else
                return _dbContext.Sessions.Include(X => X.SessionTrainer)
                    .Include(X => X.SessionCategory)
                    .Where(condition).ToList();

        }
        public Session GetSessionWithTrainerAndCategory(int sessionId)
        {

            return _dbContext.Sessions
                           .Include(x => x.SessionTrainer)
                           .Include(x => x.SessionCategory)
                           .FirstOrDefault(x => x.Id == sessionId);

        }
        public int GetCountOfBooksSlots(int sessionId)
        {
            return _dbContext.Bookings.Where(x => x.SessionId == sessionId).Count();
        }


    }
}
