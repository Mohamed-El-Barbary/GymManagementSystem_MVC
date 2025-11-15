using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // JoinDate => CreatedAt from BaseEntity
        public string Photo { get; set; } = null!;

        #region Relationships

        #region Member - HealthRecord

        public HealthRecord HealthRecord { get; set; } = null!;

        #endregion

        #region Member - Membership
        public ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();

        #endregion

        #region Member - Booking

        public ICollection<Booking> MemberBooking { get; set; } = null!;

        #endregion

        #endregion

    }
}
