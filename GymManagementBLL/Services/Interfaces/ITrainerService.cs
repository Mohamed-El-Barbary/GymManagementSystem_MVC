using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {

        IEnumerable<TrainerViewModel> GetAllTrainers();

        TrainerViewModel? GetTrainerDetails(int trainerId);

        TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);

        bool CreateTrainer(CreateTrainerViewModel createdTrainer);

        bool UpdateTrainer(int trainerId, TrainerToUpdateViewModel trainerToUpdate);

        bool RemoveTrainer(int trainerId);

    }
}
