using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        #region GetAllMembers

        public IActionResult Index()
        {
            var mebmerships = _membershipService.GetAllMemberships();
            return View(mebmerships);
        }

        #endregion

        #region CreateMembership

        public IActionResult Create()
        {
            LoadDropdownsData();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateMembershipViewModel createdMember)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Cannot create membership. Member may have an active membership.";
                LoadDropdownsData();
                return View(createdMember);
            }

            bool isCreated = _membershipService.CreateMembership(createdMember);

            if (isCreated)
                TempData["SuccessMessage"] = "Member added successfully.";
            else
            {
                TempData["ErrorMessage"] = "cannot to Create membership. Member may have an active membership.";
                LoadDropdownsData();
                return View(createdMember);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region CancelMembership

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var result = _membershipService.DeleteMembership(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Membership cancelled successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to cancel membership.";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region HelperMethod

        private void LoadDropdownsData()
        {
            var members = _membershipService.GetMembersForDropDown();
            ViewBag.members = new SelectList(members, "Id", "Name");

            var plans = _membershipService.GetPlansForDropDown();
            ViewBag.plans = new SelectList(plans, "Id", "Name");

        }

        #endregion

    }
}
