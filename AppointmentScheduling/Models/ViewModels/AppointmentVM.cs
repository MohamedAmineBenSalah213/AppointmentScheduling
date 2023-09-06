using System.ComponentModel.DataAnnotations;

namespace AppointementScheduling.Models.ViewModels
{
    public class AppointmentVM
    {
      
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "M/dd/yyyy h:mm:ss")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Duration { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public bool DoctorApproved { get; set; }
        public string? AdminId { get; set; }
        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }

        public string? AdminName { get; set; }
        public bool isforClient { get; set; }
    }
}

