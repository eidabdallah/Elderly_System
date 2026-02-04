using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderlySystem.DAL.Model
{
    public class Activity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "اسم النشاط مطلوب")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "اسم النشاط يجب أن يكون بين 3 و 150 حرف.")]
        public string ActivityName { get; set; } = null!;
        [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "موقع النشاط مطلوب.")]
        public string Location { get; set; } = null!;
        [Required(ErrorMessage = "تاريخ النشاط مطلوب")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "وقت بدء النشاط مطلوب.")]
        [StringLength(20, ErrorMessage = "وقت النشاط يجب ألا يتجاوز 20 حرف.")]
        public string StartTime { get; set; } = null!;
        public string AdminId { get; set; } = null!;
        public ApplicationUser Admin { get; set; } = null!;
        [MinLength(1, ErrorMessage = "يجب إضافة جهة منظمة أو مشارك واحد على الأقل.")]
        public ICollection<Participant> ActivityOrganizations { get; set; } = new List<Participant>();


    }
}
