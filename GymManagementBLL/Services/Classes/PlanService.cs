using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {

        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region GetAllPlans

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();

            if (plans == null || !plans.Any()) return [];

            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);

        }

        #endregion

        #region GetPlanDetails

        public PlanViewModel? GetPlanDetails(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan is null) return null;

            return _mapper.Map<PlanViewModel>(plan);

        }

        #endregion

        #region UpdatePLan

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null || plan.IsActive == false || HasActivateMemberships(planId))
                return null;

            return _mapper.Map<UpdatePlanViewModel>(plan);
        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan)
        {

            try
            {

                var planRepo = _unitOfWork.GetRepository<Plan>();
                var existingPlan = planRepo.GetById(planId);
                if (existingPlan is null || HasActivateMemberships(planId)) return false;

                _mapper.Map(updatedPlan, existingPlan);

                planRepo.Update(existingPlan);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        #region PlanStatus

        public bool PlanStatusToggle(int planId)
        {

            try
            {

                var planRepo = _unitOfWork.GetRepository<Plan>();
                var plan = planRepo.GetById(planId);
                if (plan == null || HasActivateMemberships(planId)) return false;

                plan.IsActive = !plan.IsActive;

                planRepo.Update(plan);

                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        #region Helper Method

        private bool HasActivateMemberships(int planId)
        {
            var activateMemberships = _unitOfWork.GetRepository<Membership>().GetAll(
                mp => mp.PlanId == planId && mp.Status == "Active").Any();

            if (activateMemberships)
                return true;
            else
                return false;
        }

        #endregion

    }
}
