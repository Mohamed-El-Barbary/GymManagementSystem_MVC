using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }

        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionDetails(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = $"No Session found with ID: {id}. Please check and try again.";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }

        public IActionResult Create()
        {
            LoadCategoriesDropDown();
            LoadTrainersDropDown();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateSessionViewModel createdSession)
        {
            if (!ModelState.IsValid)
            {
                LoadCategoriesDropDown();
                LoadTrainersDropDown();
                return View(createdSession);
            }

            var result = _sessionService.CreateSession(createdSession);

            if (result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("DataMissed", "Failed to create session. Please verify trainer and category exist.");
                LoadCategoriesDropDown();
                LoadTrainersDropDown();
                return View(createdSession);
            }

        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionToUpdate(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = $"Can't Update Session ";
                return RedirectToAction(nameof(Index));
            }

            LoadTrainersDropDown();
            return View(session);

        }

        [HttpPost]
        public IActionResult Edit(int id, UpdateSessionViewModel updatedSession)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Can't Update Session");
                LoadTrainersDropDown();
                return View(updatedSession);
            }

            var result = _sessionService.UpdateSession(id, updatedSession);

            if (result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("DataMissed", "Failed to Update session");
                LoadTrainersDropDown();
                return View(updatedSession);
            }

        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var sessionToDelete = _sessionService.GetSessionDetails(id);

            if (sessionToDelete == null)
            {
                TempData["ErrorMessage"] = $"No Session found with ID: {id}. Please check and try again.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View(sessionToDelete);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed([FromForm]int id)
        {
            var result = _sessionService.RemoveSession(id);

            if (result)
                TempData["SuccessMessage"] = "Session Removed Successfully";
            else
                TempData["ErrorMessage"] = "Session Not Removed";

            return RedirectToAction(nameof(Index));
        }

        #region HelperMethod

        private void LoadCategoriesDropDown()
        {
            var categories = _sessionService.GetCategoriesForDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        private void LoadTrainersDropDown()
        {
            var trainers = _sessionService.GetTrainersForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }

        #endregion

    }
}
