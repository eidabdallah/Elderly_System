namespace ElderlySystem.DAL.Model
{
    public class Sponsor : ApplicationUser
    {
        public string? Note { get; set; }
        //public ICollection<ElderlySponsor> ElderlySponsors { get; set; } = new List<ElderlySponsor>();

    }
}
