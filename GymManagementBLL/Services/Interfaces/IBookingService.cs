using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        IEnumerable<MemberForSessionViewModel> GetMembersSession(int sessionId);
        IEnumerable<MemberSelectListViewModel> GetMembersForDropDown(int sessionId);
        bool CreateNewBooking(CreateBookingViewModel CreatedBooking);
        bool CancelBooking(int MemberId,int sessionId);
        bool IsMemberAttended(int MemberId, int sessionId);

    }
}
