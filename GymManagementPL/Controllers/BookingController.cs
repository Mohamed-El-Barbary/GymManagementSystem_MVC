using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IActionResult Index()
        {
            var sessionToBook = _bookingService.GetAllSessions();
            return View(sessionToBook);
        }

        public IActionResult GetMembersForUpcomingSession(int id)
        {
            var members = _bookingService.GetMembersSession(id);
            return View(members);
        }

        public IActionResult Create(int id)
        {
            var members = _bookingService.GetMembersForDropDown(id);
            ViewBag.members = new SelectList(members, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBookingViewModel createdBooking)
        {
            var result = _bookingService.CreateNewBooking(createdBooking);

            if (result)
                TempData["SuccessMessage"] = "Booking Created successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Create Booking.";

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = createdBooking.SessionId });
        }

        public IActionResult GetMembersForOngoingSession(int id)
        {
            var members = _bookingService.GetMembersSession(id);
            return View(members);
        }

        [HttpPost]
        public IActionResult Attended(int MemberId, int SessionId)
        {
            var result = _bookingService.IsMemberAttended(MemberId, SessionId);

            if (result)
                TempData["SuccessMessage"] = "Member attended successfully";
            else
                TempData["ErrorMessage"] = "Member attendance can't be marked";

            return RedirectToAction(nameof(GetMembersForOngoingSession), new { id = SessionId });
        }

        public IActionResult Cancel(int MemberId, int SessionId)
        {
            var result = _bookingService.CancelBooking(MemberId, SessionId);

            if (result)
                TempData["SuccessMessage"] = "Booking cancelled successfully";
            else
                TempData["ErrorMessage"] = "Booking can't be cancelled";

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = SessionId });
        }

    }
}


