using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Session : BaseEntity
    {

        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region Relationships

        #region Session - Booking

        public ICollection<Booking> BookingMembers { get; set; } = null!;

        #endregion

        #region Session - Category
        public int CategoryId { get; set; }
        public Category SessionCategory { get; set; } = null!;
        #endregion

        #region Session - Trainer

        public int TrainerId { get; set; }
        public Trainer SessionTrainer { get; set; } = null!;

        #endregion

        #endregion

    }
}
