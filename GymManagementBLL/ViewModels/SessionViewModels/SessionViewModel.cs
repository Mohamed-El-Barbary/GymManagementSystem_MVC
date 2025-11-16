namespace GymManagementBLL.ViewModels.SessionViewModels
{
	public class SessionViewModel
	{
		public int Id { get; set; }
		public string Description { get; set; } = null!;
		public int Capacity { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string TrainerName { get; set; } = null!;
		public string CategoryName { get; set; } = null!;
		public int AvailableSlots { get; set; }

		// Computed properties
		public string DateDisplay => $"{StartDate:MMM dd , yyyy}";
		public string TimeRangeDisplay => $"{StartDate:hh:mm tt} - {EndDate:hh:mm tt}";
		public TimeSpan Duration => EndDate - StartDate;
		public string Status
		{
			get
			{
				if (StartDate > DateTime.UtcNow)
					return "Upcoming";
				else if (StartDate <= DateTime.UtcNow && EndDate >= DateTime.UtcNow)
					return "Ongoing";
				else
					return "Completed";
			}
		}
	}
}
