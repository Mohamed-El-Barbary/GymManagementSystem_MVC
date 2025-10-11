using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        // HireDate => CreatedAt from BaseEntity
        public Specialties Specialties { get; set; }

        #region Trainer - Session

        public ICollection<Session> TrainerSessions { get; set; } = null!;

        #endregion

    }
}
