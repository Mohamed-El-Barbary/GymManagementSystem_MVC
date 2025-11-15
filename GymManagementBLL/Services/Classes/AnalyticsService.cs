using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AnalyticsViewModel GetAnalyticsData()
        {

            var sessions = _unitOfWork.GetRepository<Session>().GetAll();

            return new AnalyticsViewModel()
            {
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                ActiveMembers = _unitOfWork.GetRepository<Membership>().GetAll(x => x.Status == "Active").Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = sessions.Count(x => x.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now),
                CompletedSessions = sessions.Count(x => x.EndDate < DateTime.Now)
            };
        }
    }
}
