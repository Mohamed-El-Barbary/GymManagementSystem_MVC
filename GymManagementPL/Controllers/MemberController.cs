using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {

        #region Fields

        private readonly IMemberService _memberService;

        #endregion

        #region Constructors

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        #endregion

        #region GetAllMembers

        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        #endregion

        #region CreateMember

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateMember(CreateMemberViewModel createdMember)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Fields");
                return View(nameof(Create), createdMember);
            }

            bool result = _memberService.CreateMember(createdMember);

            if (result)
                TempData["SuccessMessage"] = $"Member Added Successfully";
            else
                TempData["ErrorMessage"] = $"Member Failed To Added , Phone Number Or Email already exists";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Get Member Details

        public IActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var memberDetails = _memberService.GetMemberDetails(id);

            if (memberDetails == null)
            {
                TempData["ErrorMessage"] = $"No member found with ID: {id}. Please check and try again.";
                return RedirectToAction(nameof(Index));
            }

            return View(memberDetails);

        }

        public IActionResult HealthRecordDetails(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var memberHealthRecordData = _memberService.GetMemberHealthRecord(id);

            if (memberHealthRecordData == null)
            {
                TempData["ErrorMessage"] = $"No member found with ID: {id}. Please check and try again.";
                return RedirectToAction(nameof(Index));
            }

            return View(memberHealthRecordData);

        }

        #endregion

        #region EditMember

        public IActionResult EditMember(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberToUpdate(id);

            if (member == null)
            {
                TempData["ErrorMessage"] = $"No member found with ID: {id}. Please check and try again.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public IActionResult EditMember([FromRoute] int id, MemberToUpdateViewModel memberToUpdate)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Email Or Phone Exist");
                return View(memberToUpdate);
            }
            bool result = _memberService.UpdateMember(id, memberToUpdate);

            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfully.";
            else
                TempData["ErrorMessage"] = "Member Failed To Update .";

            return RedirectToAction(nameof(Index));

        }

        #endregion

        #region DeleteMember

        public IActionResult Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID. Please provide a valid identifier.";
                return RedirectToAction(nameof(Index));
            }

            var memberDetails = _memberService.GetMemberDetails(id);

            if (memberDetails == null)
            {
                TempData["ErrorMessage"] = $"No member found with ID: {id}. Please check and try again.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();
        }

        public IActionResult DeleteConfirmed([FromForm] int id)
        {

            var result = _memberService.RemoveMember(id);

            if (result)
                TempData["SuccessMessage"] = "Member Removed Successfully";
            else
                TempData["ErrorMessage"] = "Member Not Removed";

            return RedirectToAction(nameof(Index));

        }

        #endregion

    }
}
