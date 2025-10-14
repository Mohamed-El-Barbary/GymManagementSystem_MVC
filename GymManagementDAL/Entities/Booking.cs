using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Booking : BaseEntity
    {

        // BookingDay => CreatedAt from BaseEntity
        public bool IsAttended { get; set; }

        #region Navigation Property

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;

        #endregion

    }
}
