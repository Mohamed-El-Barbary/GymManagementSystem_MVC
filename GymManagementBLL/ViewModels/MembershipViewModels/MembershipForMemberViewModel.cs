namespace GymManagementBLL.ViewModels.MembershipViewModels
{
	public class MembershipForMemberViewModel
	{
		public string MemberName { get; set; } = null!;
		public string PlanName { get; set; } = null!;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string? Status { get; set; }
		public int? RemainingDays
		{
			get
			{
				return (EndDate - DateTime.Now).Days;
			}
		}
	}
}
