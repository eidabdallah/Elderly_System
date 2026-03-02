using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class WeeklyElderMealsResponse
    {
        public List<string> Dates { get; set; } = new();
        public List<WeeklyElderMealRowDto> Rows { get; set; } = new();
    }
}
