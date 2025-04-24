using EventManagementSystem.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementSystem.Models.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string? EventTitle { get; set; }
        public string? EventDescription { get; set; }
        public string? EventLocation { get; set; }
        public DateTime EventDate { get; set; }
        public string? EventCategory { get; set; }
        public int AttendeeCount { get; set; }
        public string? ImagePath { get; set; }
        public string? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public AppUser? Owner { get; set; }

    public string ComputedStatus
        {
            get
            {
                if (EventDate < DateTime.Now)
                    return "Completed";
                else if (EventDate.Date == DateTime.Now.Date)
                    return "Ongoing";
                else
                    return "Upcoming";
            }
        }
    }
}
    