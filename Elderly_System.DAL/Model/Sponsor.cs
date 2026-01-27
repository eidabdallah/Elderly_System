using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class Sponsor : ApplicationUser
    {
        [StringLength(500, ErrorMessage = "الملاحظة يجب ألا تتجاوز 500 حرف.")]
        public string? Note { get; set; }

        [MinLength(1, ErrorMessage = "يجب أن يكون الكافل مرتبطًا بمسن واحد على الأقل.")]
        public ICollection<ElderlySponsor> ElderlySponsors { get; set; } = new List<ElderlySponsor>();

    }
}
 