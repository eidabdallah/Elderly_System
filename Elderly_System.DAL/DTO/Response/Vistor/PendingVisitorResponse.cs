using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.DTO.Response.Vistor
{
    public class PendingVisitorResponse
    {
        public int RequestId { get; set; }
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = "";

        public int VisitorId { get; set; }
        public string VisitorName { get; set; } = "";
        public string VisitorPhone { get; set; } = "";
        public string Date { get; set; } = "";      
        public string StartTime { get; set; } = ""; 
        public string EndTime { get; set; } = "";  
    }
}
