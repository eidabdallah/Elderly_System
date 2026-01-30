namespace EA_Ecommerce.DAL.utils.SeedData
{
    public interface ISeedData
    {
        Task IdentityDataSeedingAsync();
        Task SeedShiftsAsync();
    }
}
