using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {

        IEnumerable<SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionDetails(int sessionId);

        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);

        bool CreateSession(CreateSessionViewModel createdSession);
        bool UpdateSession(int sessionId, UpdateSessionViewModel updateSession);

        bool RemoveSession(int sessionId);
        IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown();
        IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown();
    }
}
