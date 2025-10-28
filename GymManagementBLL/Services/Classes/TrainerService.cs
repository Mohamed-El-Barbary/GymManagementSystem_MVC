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

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {

            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return [];
            var mappedTrainers = _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
            return mappedTrainers;

        }

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

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {

            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null) return null;

            var mappedTrainer = _mapper.Map<TrainerViewModel>(trainer);

            return mappedTrainer;

        }

        public TrainerToUpdateViewModel? TrainerToUpdateViewModel(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null) return null;

            var mappedTrainer = _mapper.Map<TrainerToUpdateViewModel>(trainer);

            return mappedTrainer;
        }

        public bool UpdateTrainer(int trainerId, TrainerToUpdateViewModel trainerToUpdate)
        {

            if (IsMailExist(trainerToUpdate.Email) || IsPhoneExist(trainerToUpdate.Phone))
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
