using AppointementScheduling.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointementScheduling.Helper
{
    public static class Helper
    {
        public const  string admin = "Admin";
        public static string patient = "Patient";
        public static string doctor = "Doctor";


public static string appointmentAdded = "Appointment added successfully";
public static string appointmentUpdated = "Appointment updated succesefully.";
public static string appointmentDeleted = "Appointment deleted successfully.";
public static string appointmentExists = "Appointment for selected date and time already exists.";
public static string appointmentNotExists = "Appointment not exists.";
public static string appointmentAddError = "Something went vront, Plasse try again.";
public static string appointmentUpdatEcror = "Something went wront, Please try again";

public static string somethingWentWrong = "Something went wrong, Please try again.";
        public static string meetingConfirm = "Meeting confirm successfully.";
        public static string meetingConfirmError = "Error while confirming meeting.";
        public static int failure_code = 000;
        public static int success_code = 1;
        public static List<SelectListItem> GetRolesForDropDown(bool isAdmin)
        {
            if (isAdmin)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.admin,Text=Helper.admin}
                };
            }
            else
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.patient,Text=Helper.patient},
                    new SelectListItem{Value=Helper.doctor,Text=Helper.doctor}
                };
            }
        }
        public static List<SelectListItem> GetTimeDropDown()
        {

            int minute = 60;
            List<SelectListItem> duration = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + "Hr" });
                minute = minute + 30;
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + "Hr 30 min" });
                minute = minute + 30;
            }


            return duration;
        }

    }
}
