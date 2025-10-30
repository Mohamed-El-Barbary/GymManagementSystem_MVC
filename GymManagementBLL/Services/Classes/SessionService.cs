using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {

        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 

        #endregion

        #region Constructor

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        } 

        #endregion

        #region CreateSession

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {

                if (!IsTrainerExist(createdSession.TrainerId)) return false;
                if (!IsCategoryExist(createdSession.CategoryId)) return false;
                if (!IsValidDateRange(createdSession.StartDate, createdSession.EndDate)) return false;

                if (createdSession is null) return false;

                var mappedSession = _mapper.Map<Session>(createdSession);

                _unitOfWork.GetRepository<Session>().Add(mappedSession);
                mappedSession.CreatedAt = DateTime.Now;
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region GetALlSessions

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory().OrderByDescending(x => x.StartDate);

            if (sessions == null || !sessions.Any()) return [];

            var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in mappedSessions)
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBooksSlots(session.Id);

            return mappedSessions;

        }

        #endregion

        #region GetSessionDetails

        public SessionViewModel? GetSessionDetails(int sessionId)
        {

            var sessoin = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);

            if (sessoin == null) return null;

            var mappedSession = _mapper.Map<SessionViewModel>(sessoin);

            mappedSession.AvailableSlots = mappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBooksSlots(sessionId);
            return mappedSession;

        }

        #endregion

        #region UpdateSession

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var sessionRepo = _unitOfWork.GetRepository<Session>();
            var session = sessionRepo.GetById(sessionId);

            if (!IsSessionValidToUpdate(session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(session);

        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel updateSession)
        {
            try
            {

                var sessionRepo = _unitOfWork.GetRepository<Session>();
                var session = sessionRepo.GetById(sessionId);

                if (!IsTrainerExist(updateSession.TrainerId)) return false;
                if (!IsValidDateRange(updateSession.StartDate, updateSession.EndDate)) return false;
                if (!IsSessionValidToUpdate(session!)) return false;


                _mapper.Map(updateSession, session);
                session.UpdatedAt = DateTime.Now;

                sessionRepo.Update(session);

                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }


        }

        #endregion

        #region RemoveSession

        public bool RemoveSession(int sessionId)
        {
            try
            {

                var sessionRepo = _unitOfWork.GetRepository<Session>();
                var session = sessionRepo.GetById(sessionId);

                if (!IsSessionValidToRemove(session!)) return false;

                sessionRepo.Delete(session!);

                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Helper Method

        private bool IsTrainerExist(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(x => x.Id == TrainerId).Any();
        }

        private bool IsCategoryExist(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetAll(x => x.Id == CategoryId).Any();
        }

        private bool IsValidDateRange(DateTime startDate, DateTime EndDate)
        {
            return EndDate > startDate && EndDate > DateTime.Now;
        }

        private bool IsSessionValidToUpdate(Session session)
        {
            return session != null &&
                   session.EndDate >= DateTime.Now &&
                   session.StartDate > DateTime.Now &&
                   _unitOfWork.SessionRepository.GetCountOfBooksSlots(session.Id) == 0;
        }

        private bool IsSessionValidToRemove(Session session)
        {
            return session != null &&
            session.EndDate <= DateTime.Now &&
            _unitOfWork.SessionRepository.GetCountOfBooksSlots(session.Id) == 0;
        }

        #endregion

    }
}
