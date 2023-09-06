using AppointementScheduling.Helper;
using AppointementScheduling.Models;
using AppointementScheduling.Models.ViewModels;

namespace AppointementScheduling.Services
{
    public class AppointementService : IAppointementService
    {
        private readonly ApplicationDBcontext dBcontext;
        public AppointementService(ApplicationDBcontext Bcontext)
        {
            if (Bcontext == null)
            {
                throw new ArgumentNullException(nameof(Bcontext), "The ApplicationDBcontext instance cannot be null.");
            }

            dBcontext = Bcontext;

        }
        public async Task<int> ConfirmEvent(int id)
        {
            try
            {
                var appointment = dBcontext.Appointments.FirstOrDefault(x => x.Id == id);
                Console.WriteLine(appointment);
                if (appointment != null)
                {
                    appointment.DoctorApproved = true;
                    
                    
                    return await dBcontext.SaveChangesAsync();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<int> Delete(int id)
        {
            var appointment = dBcontext.Appointments.FirstOrDefault(x => x.Id == id);
            Console.WriteLine(appointment);
            if (appointment != null)
            {
                dBcontext.Appointments.Remove(appointment);
                return await dBcontext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startTime = model.StartDate;
            var endTime = startTime.AddMinutes(Convert.ToDouble(model.Duration));
            if (model != null && model.Id > 0)
            {
                //update
                //update
                var appointment = dBcontext.Appointments.FirstOrDefault(x => x.Id == model.Id);
                appointment.Title = model.Title;
                appointment.Description = model.Description;
                appointment.StartDate = startTime;
                appointment.EndDate = endTime;
                appointment.Duration = model.Duration;
                appointment.DoctorId = model.DoctorId;
                appointment.PatientId = model.PatientId;
                appointment.DoctorApproved = false;
                appointment.AdminId = model.AdminId;
                await dBcontext.SaveChangesAsync();
                return 1;
            }
            else
            {
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startTime,
                    EndDate = endTime,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    DoctorApproved = false,
                    AdminId = model.AdminId,



                };
                dBcontext.Add(appointment);
                await dBcontext.SaveChangesAsync();
                return 2;
            }

        }

       

        public List<AppointmentVM> GetAppointmentDoctorsByID(string doctorsId)
        {
          try  {
                var appointments = dBcontext.Appointments
                    .Where(a => a.DoctorId == doctorsId)
                    .Select(c => new AppointmentVM()
                    {
                        Description = c.Description,
                        Title = c.Title,
                        StartDate=c.StartDate,
                        EndDate=c.EndDate,
                        Duration=c.Duration,
                        DoctorId=c.DoctorId,
                        PatientId=c.PatientId,
                        DoctorApproved=c.DoctorApproved,
                        Id=c.Id
                        
                    })
                    .ToList();

                Console.WriteLine(appointments);
                return appointments;
            }
          catch (Exception ex)
            {
                Console.WriteLine( ex.Message);
                throw; 
            }
          
        }

        public List<AppointmentVM> GetAppointmentPatientsByID(string patinetsId)
        {
            return dBcontext.Appointments.Where(a => a.PatientId == patinetsId).ToList().Select(c => new AppointmentVM()
            {
              //  Id = c.Id,
                Description = c.Description,
               /* StartDate = c.StartDate,
                EndDate = c.EndDate,
                Title = c.Title,
                Duration = c.Duration,*/
            }).ToList();
        }

        public AppointmentVM GetById(int id) {
            try
            {
                var appointments = dBcontext.Appointments
                    .Where(a => a.Id == id)
                    .Select(c => new AppointmentVM()
                    {
                        Description = c.Description,
                        Title = c.Title,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                        Duration = c.Duration,
                        DoctorId = c.DoctorId,
                        PatientId = c.PatientId,
                        DoctorApproved = c.DoctorApproved,
                        Id = c.Id,
                        PatientName = dBcontext.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                        DoctorName = dBcontext.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()

                    })
                    .SingleOrDefault();

                Console.WriteLine(appointments);
                return appointments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public List<DoctorVM> GetDoctorList()
        {
            var doctors = (from user in dBcontext.Users
                           join userRoles in dBcontext.UserRoles on user.Id equals userRoles.UserId
                           join roles in dBcontext.Roles.Where(x => x.Name == Helper.Helper.doctor) on userRoles.RoleId equals roles.Id


                           select new DoctorVM { Id = user.Id, Name = user.Name }).ToList();
            return doctors;
        }

        public List<PatientVM> GetPatientList()
        {
            var doctors = (from user in dBcontext.Users
                           join userRoles in dBcontext.UserRoles on user.Id equals userRoles.UserId
                           join roles in dBcontext.Roles.Where(x => x.Name == Helper.Helper.patient) on userRoles.RoleId equals roles.Id


                           select new PatientVM { Id = user.Id, Name = user.Name }).ToList();
            return doctors;
        }

   
    }
}
