using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class Participant
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "اسم الجهة المشاركة مطلوب.")]
        public string OrganizationName { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}
