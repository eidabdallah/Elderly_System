using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.DAL.DTO.Response.Nurse
{
    public class NurseWeeklyShiftDto
    {
        public List<string> Dates { get; set; } = new(); // yyyy-MM-dd
        public Dictionary<string, string> Days { get; set; } = new(); 
    }
}
