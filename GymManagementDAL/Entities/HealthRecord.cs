using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class HealthRecord : BaseEntity
    {

        // LastUpdated => UpdatedAt from BaseEntity 

        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }


    }
}
