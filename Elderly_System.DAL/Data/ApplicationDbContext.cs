using Elderly_System.DAL.Model;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElderlySystem.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<Elderly> Elderlies { get; set; }
        public DbSet<ElderlySponsor> ElderlySponsors { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<ElderlyVisitor> ElderlyVisitors { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
        public DbSet<ResidentStay> ResidentStays { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<NurseShiftAssignment> NurseShiftAssignments { get; set; }
        public DbSet<ElderMeal> ElderMeals { get; set; }
        public DbSet<MedicalReport> MedicalReports { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicalReportMedicine> MedicalReportMedicines { get; set; }
        public DbSet<DrugPlan> DrugPlans { get; set; }
        public DbSet<Medication> Medications { get; set; }
        //public DbSet<CheckList> CheckLists { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");
            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<Sponsor>().ToTable("Sponsors");
            builder.Entity<Nurse>().ToTable("Nurses");

        }
    }
}
