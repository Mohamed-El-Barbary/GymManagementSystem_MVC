using AutoMapper;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {

        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        #endregion

        #region Constructor

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        #endregion

        #region GetAllMembers
        public IEnumerable<MemberViewModel> GetAllMembers()
        {

            var members = _unitOfWork.GetRepository<Member>().GetAll();

            if (members == null || !members.Any()) return [];

            return _mapper.Map<IEnumerable<MemberViewModel>>(members);

        }

        #endregion

        #region GetMemberHealthRecord

        public HealthRecordViewModel? GetMemberHealthRecord(int id)
        {

            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(id);

            if (memberHealthRecord is null) return null;

            return _mapper.Map<HealthRecordViewModel>(memberHealthRecord);

        }


        #endregion

        #region GetMemberDetails

        public MemberViewModel? GetMemberDetails(int id)
        {
            var repo = _unitOfWork.GetRepository<Member>();
            var member = repo.GetById(id);

            if (member is null) return null;

            var mappedMember = _mapper.Map<MemberViewModel>(member);

            var ActiveMembership = _unitOfWork.GetRepository<Membership>()
                .GetAll(m => m.MemberId == id && m.Status == "Active").FirstOrDefault();

            if (ActiveMembership is not null)
            {
                var activePlan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId);
                if (activePlan is null) return null;
                mappedMember.PlanName = activePlan?.Name;
                mappedMember.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();
                mappedMember.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
            }

            return mappedMember;

        }

        #endregion

        #region CreateMember

        public bool CreateMember(CreateMemberViewModel createdMember)
        {

            try
            {

                if (IsMailExist(createdMember.Email) || IsPhoneExist(createdMember.Phone))
                    return false;

                var photoName = _attachmentService.Upload("members", createdMember.PhotoFile);
                if (string.IsNullOrEmpty(photoName)) return false;

                var member = _mapper.Map<Member>(createdMember);
                member.Photo = photoName;

                _unitOfWork.GetRepository<Member>().Add(member);

                bool isCreated =  _unitOfWork.SaveChanges() > 0;

                if (!isCreated)
                    _attachmentService.Delete(photoName, "members");

                return isCreated;

            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        #region UpdateMember

        public MemberToUpdateViewModel? GetMemberToUpdate(int id)
        {

            var member = _unitOfWork.GetRepository<Member>().GetById(id);

            if (member is null) return null;

            return _mapper.Map<MemberToUpdateViewModel>(member);

        }

        public bool UpdateMember(int id, MemberToUpdateViewModel updatedMember)
        {

            var emailExist = _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == updatedMember.Email && x.Id != id);
            var phoneExist = _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == updatedMember.Phone && x.Id != id);

            if (emailExist.Any() || phoneExist.Any())
                return false;

            var repo = _unitOfWork.GetRepository<Member>();

            var existingMember = repo.GetById(id);
            if (existingMember is null) return false;


            _mapper.Map(updatedMember, existingMember);

            repo.Update(existingMember);

            return _unitOfWork.SaveChanges() > 0;

        }


        #endregion

        #region RemoveMember

        public bool RemoveMember(int id)
        {
            if (id <= 0) return false;
            var repo = _unitOfWork.GetRepository<Member>();
            var member = repo.GetById(id);
            if (member is null) return false;

            var sessionIds = _unitOfWork.GetRepository<Booking>()
                .GetAll(x => x.MemberId == id).Select(x => x.SessionId);

            var hasFutureSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(S => sessionIds.Contains(S.Id) && S.StartDate > DateTime.Now).Any();

            if (hasFutureSessions) return false;

            var memberships = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == id).ToList();

            try
            {
                if (memberships.Any())
                {
                    foreach (var membership in memberships)
                        _unitOfWork.GetRepository<Membership>().Delete(membership);
                }

                repo.Delete(member);
                bool isDeleted = _unitOfWork.SaveChanges() > 0;

                if (isDeleted) 
                    _attachmentService.Delete(member.Photo, "members") ;

                return isDeleted;

            }
            catch (Exception)
            {
                return false;
            }


        }

        #endregion

        #region HelperMethod

        private bool IsMailExist(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        }

        #endregion
    }
}
