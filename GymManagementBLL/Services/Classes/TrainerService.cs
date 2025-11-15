using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {

        #region Fileds

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion
        
        #region GetAllTrainers

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {

            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return [];
            var mappedTrainers = _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
            return mappedTrainers;

        }

        #endregion

        #region CreateTrainer

        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {

            try
            {
                if (IsMailExist(createdTrainer.Email) || IsPhoneExist(createdTrainer.Phone))
                    return false;

                var trainer = _mapper.Map<Trainer>(createdTrainer);

                _unitOfWork.GetRepository<Trainer>().Add(trainer);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        #region GetTrainerDetails

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {

            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null) return null;

            var mappedTrainer = _mapper.Map<TrainerViewModel>(trainer);

            return mappedTrainer;

        }

        #endregion

        #region UpdateTrainer

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null) return null;

            var mappedTrainer = _mapper.Map<TrainerToUpdateViewModel>(trainer);

            return mappedTrainer;
        }

        public bool UpdateTrainer(int trainerId, TrainerToUpdateViewModel trainerToUpdate)
        {

            var emailExist = _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == trainerToUpdate.Email && x.Id != trainerId);
            var phoneExist = _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == trainerToUpdate.Phone && x.Id != trainerId);

            if (emailExist.Any() || phoneExist.Any())
                return false;

            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = repo.GetById(trainerId);
            if (trainer is null) return false;

            try
            {

                _mapper.Map(trainerToUpdate, trainer);
                repo.Update(trainer);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        #region RemoveTrainer

        public bool RemoveTrainer(int trainerId)
        {

            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = repo.GetById(trainerId);
            if (trainer is null) return false;

            var associatedSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now);

            if (associatedSessions.Any()) return false;

            try
            {
                repo.Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;
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
