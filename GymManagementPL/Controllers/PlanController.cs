using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {

        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public IActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }

        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Plan Can Not Be 0 Or Negative";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanDetails(id);

            if (plan == null)
            {
                TempData["ErrorMessage"] = $"Plan Of Idetifier {id} Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);

        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Plan Can Not Be 0 Or Negative";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanToUpdate(id);

            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Can not be Updated";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        [HttpPost]
        public IActionResult Edit(int id, UpdatePlanViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(updatedPlan);
            }

            var result = _planService.UpdatePlan(id, updatedPlan);

            if (result)
                TempData["SuccessMessage"] = "Plan updated successfully!";
            else
                TempData["ErrorMessage"] = "Failed to update plan.";

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Activate(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Plan Can Not Be 0 Or Negative";
                return RedirectToAction(nameof(Index));
            }

            var result = _planService.PlanStatusToggle(id);

            if (result)
                TempData["SuccessMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Failed to Change plan Status";

            return RedirectToAction(nameof(Index));

        }
    }
}
