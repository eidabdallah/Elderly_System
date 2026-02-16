namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class AddResidentStayRequest
    {
        public int ElderlyId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
    }
}
