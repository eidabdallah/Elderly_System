using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderlySystem.DAL.Model
{
    [Index(nameof(Phone), IsUnique = true)]

    public class Visitor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "اسم الزائر مطلوب.")]
        [StringLength(100, ErrorMessage = "الاسم يجب ألا يتجاوز 100 حرف.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")]
        public string Phone { get; set; } = null!;
        public ICollection<ElderlyVisitor> ElderlyVisitors { get; set; } = new List<ElderlyVisitor>();

    }
}
