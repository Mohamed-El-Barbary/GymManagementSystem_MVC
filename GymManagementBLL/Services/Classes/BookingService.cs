using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region GetAllSessions
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessionsToBook = _unitOfWork.SessionRepository
                                      .GetAllSessionsWithTrainerAndCategory(x => x.EndDate >= DateTime.Now)
                                      .OrderByDescending(x => x.StartDate);

            if (!sessionsToBook.Any()) return [];

            var mappedSessionsToBook = _mapper.Map<IEnumerable<SessionViewModel>>(sessionsToBook);

            foreach (var session in mappedSessionsToBook)
            {
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBooksSlots(session.Id);
            }

            return mappedSessionsToBook;
        }
        #endregion

        #region GetMembersForDropDown
        public IEnumerable<MemberSelectListViewModel> GetMembersForDropDown(int sessionId)
        {
            var membersIdHasBooking = _unitOfWork.BookingRepository
                                                 .GetAll(x => x.SessionId == sessionId)
                                                 .Select(x => x.MemberId)
                                                 .ToList();

            var members = _unitOfWork.GetRepository<Member>()
                                     .GetAll(x => !membersIdHasBooking.Contains(x.Id));
            // Get All Members That Not Have Booking Untill => !List.Contains(x.Id)

            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }
        #endregion

        #region GetMembersSession
        public IEnumerable<MemberForSessionViewModel> GetMembersSession(int sessionId)
        {
            var membersForSession = _unitOfWork.SessionRepository.GetById(sessionId);

            return _mapper.Map<IEnumerable<MemberForSessionViewModel>>(membersForSession);
        }
        #endregion

        #region CreateNewBooking
        public bool CreateNewBooking(CreateBookingViewModel createdBooking)
        {

            try
            {
                var session = _unitOfWork.SessionRepository.GetById(createdBooking.SessionId);
                if (session is null) return false;

                var hasActiveMembership = _unitOfWork.MembershipRepository
                                                     .GetAll(x => x.MemberId == createdBooking.MemberId).Any();
                if (!hasActiveMembership) return false;

                var HasAvailableSolts = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBooksSlots(createdBooking.SessionId);
                if (HasAvailableSolts == 0) return false;

                var mappedCreatedBooking = _mapper.Map<Booking>(createdBooking);

                _unitOfWork.BookingRepository.Add(mappedCreatedBooking);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        #endregion

        #region IsMemberAttended
        public bool IsMemberAttended(int memberId, int sessionId)
        {
            try
            {
                var memberSession = _unitOfWork.GetRepository<Booking>()
                                           .GetAll(X => X.MemberId == memberId && X.SessionId == sessionId)
                                           .FirstOrDefault();
                if (memberSession is null) return false;

                memberSession.IsAttended = true;
                memberSession.UpdatedAt = DateTime.Now;

                _unitOfWork.GetRepository<Booking>().Update(memberSession);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region CancelBooking
        public bool CancelBooking(int memberId, int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (session is null || session.StartDate <= DateTime.Now) return false;

                var Booking = _unitOfWork.BookingRepository.GetAll(X => X.SessionId == sessionId && X.MemberId == memberId)
                                                           .FirstOrDefault();
                if (Booking is null) return false;

                _unitOfWork.BookingRepository.Delete(Booking);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
