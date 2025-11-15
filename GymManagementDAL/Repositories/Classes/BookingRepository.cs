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
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly GymDbContext _dbContext;

        public BookingRepository(GymDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Booking> GetSessionById(int id)
        {
            return _dbContext.Bookings.Include(x => x.Member)
                                      .Where(x => x.SessionId == id).ToList();
        }
    }
}
