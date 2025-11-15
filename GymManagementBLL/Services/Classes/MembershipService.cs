using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MembershipViewModel> GetAllMemberships()
        {

            var memberships = _unitOfWork.MembershipRepository.GetAllMembershipsWithMemberAndPLan(x => x.Status == "Active");
            if (!memberships.Any()) return [];
            return _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);
        }

        public bool CreateMembership(CreateMembershipViewModel createdMembership)
        {
            try
            {
                var isMemberExist = _unitOfWork.GetRepository<Member>()
                                                   .GetAll(x => x.Id == createdMembership.MemberId)
                                                   .Any();

                var hasActiveMembership = _unitOfWork.MembershipRepository
                                                     .GetAllMembershipsWithMemberAndPLan(x => x.MemberId == createdMembership.MemberId)
                                                     .Any();

                var isPlanExistandActive = _unitOfWork.GetRepository<Plan>()
                                                      .GetAll(x => x.Id == createdMembership.PlanId && x.IsActive == true)
                                                      .Any();

                if (!isMemberExist || hasActiveMembership) return false;
                if (!isPlanExistandActive) return false;

                var membership = _mapper.Map<Membership>(createdMembership);
                var plan = _unitOfWork.GetRepository<Plan>().GetById(createdMembership.PlanId);
                membership.EndDate = DateTime.Now.AddDays(plan!.DurationDays);

                _unitOfWork.GetRepository<Membership>().Add(membership);

                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
        }


        public bool DeleteMembership(int id)
        {
            var Repo = _unitOfWork.MembershipRepository;
            var ActiveMemberships = Repo.GetAll(X => X.MemberId == id && X.Status == "Active").FirstOrDefault();
            if (ActiveMemberships is null) return false;
            Repo.Delete(ActiveMemberships);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<MemberSelectListViewModel> GetMembersForDropDown()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }

        public IEnumerable<PlanSelectListViewModel> GetPlansForDropDown()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll(x => x.IsActive == true);
            return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
        }

    }
}
