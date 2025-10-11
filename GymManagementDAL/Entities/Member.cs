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
        public string? Photo { get; set; }
    }
}
