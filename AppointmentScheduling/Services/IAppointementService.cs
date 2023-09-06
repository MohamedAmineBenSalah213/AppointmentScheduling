using AppointementScheduling.Models;
using AppointementScheduling.Models.ViewModels;

namespace AppointementScheduling.Services
{
    public interface IAppointementService
    {
        public List<DoctorVM> GetDoctorList();

        public List<PatientVM> GetPatientList();
        public  Task<int> AddUpdate(AppointmentVM model);
        public List<AppointmentVM> GetAppointmentDoctorsByID(string doctorsId);
        public List<AppointmentVM> GetAppointmentPatientsByID(string patinetsId);
        public AppointmentVM GetById(int Id);
        public Task<int> Delete(int id);

        public Task<int> ConfirmEvent(int id);
    }
}
