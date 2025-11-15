using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        public IActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = $"No member found with ID: {id}. Please check and try again.";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateTrainer(CreateTrainerViewModel createdTrainer)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Fields");
                return View(nameof(Create), createdTrainer);
            }

            var result = _trainerService.CreateTrainer(createdTrainer);

            if (result)
                TempData["SuccessMessage"] = $"Member Added Successfully";
            else
                TempData["ErrorMessage"] = $"Member Failed To Added , Phone Number Or Email already exists";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerToUpdate(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = $"No member found with ID: {id}. Please check and try again.";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);

        }

        [HttpPost]
        public IActionResult EditTrainer([FromRoute] int id, TrainerToUpdateViewModel trainerToUpdate)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Email Or Phone Exist");
                return View(trainerToUpdate);
            }

            var result = _trainerService.UpdateTrainer(id, trainerToUpdate);

            if (result)
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Failed To Updated";

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete([FromRoute] int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var member = _trainerService.GetTrainerDetails(id);

            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = $"No member found with ID: {id}. Please check and try again.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;
            return View(trainer);
        }

        public IActionResult DeleteConfirmed([FromForm] int id)
        {

            var result = _trainerService.RemoveTrainer(id);

            if (result)
                TempData["SuccessMessage"] = "Member Removed Successfully";
            else
                TempData["ErrorMessage"] = "Member Not Removed";

            return RedirectToAction(nameof(Index));

        }
    }
}
