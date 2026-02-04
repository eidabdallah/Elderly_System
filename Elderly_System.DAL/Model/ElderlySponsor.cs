using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    [PrimaryKey(nameof(ElderlyId), nameof(SponsorId))]
    public class ElderlySponsor
    {
        public int ElderlyId { get; set; }
        public string SponsorId { get; set; } = null!;
        [Required(ErrorMessage = "معرّف الكفيل مطلوب.")]
        public string KinShip {  get; set; } = null!;
        [Required(ErrorMessage = "درجة القرابة مطلوبة.")]
        public string Degree { get; set; } = null!;
        public Elderly Elderly { get; set; } = null!;
        public Sponsor Sponsor { get; set; } = null!;

    }
}
