using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class Sponsor : ApplicationUser
    {
        [Required(ErrorMessage ="يجب تحديد درجة الكفيل")]        
        public SponsorDegree Degree { get; set; }

        [MinLength(1, ErrorMessage = "يجب أن يكون الكافل مرتبطًا بمسن واحد على الأقل.")]
        public ICollection<ElderlySponsor> ElderlySponsors { get; set; } = new List<ElderlySponsor>();

    }
}
 