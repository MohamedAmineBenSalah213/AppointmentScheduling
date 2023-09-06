using AppointementScheduling.Models;
using AppointementScheduling.Models.ViewModels;
using AppointementScheduling.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppointementScheduling.Controllers.Api
{
    //Api EndPoint
    [ApiController]
    [Route("/api/Appointment")]
  
    public class AppointmentApiController : Controller
    {
        private readonly  IAppointementService _appointementService;
        private readonly IHttpContextAccessor _http;
        private readonly string userID;
        private readonly string role;
        public AppointmentApiController(IAppointementService appointementService,IHttpContextAccessor httpContext)
        {
            _appointementService = appointementService;
            _http = httpContext;
            //this.userID = userID;
            //this.role = role;
            userID = _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                role = _http.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }
        [HttpPost]
        [Route("SaveCalendarDate")]
        public IActionResult SaveCalendarDate(AppointmentVM data)
        {
            
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = _appointementService.AddUpdate(data).GetAwaiter().GetResult();
                if (commonResponse.status == 1)
                {
                    commonResponse.message = Helper.Helper.appointmentUpdated;
                }
                if (commonResponse.status == 2)
                {
                    commonResponse.message = Helper.Helper.appointmentAdded;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                commonResponse.message = e.Message;
                commonResponse.status = Helper.Helper.failure_code;
            }
            return Ok(commonResponse);
        }
        [Route("test")]
        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok("test");
        }
        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData( string doctorId)
        {
            CommonResponse<List<AppointmentVM   >> commonResponse= new CommonResponse<List<AppointmentVM>>();
            try
            {
                if (role == Helper.Helper.patient)
                {
                   // commonResponse.dataenum = _appointementService.GetAppointmentPatientsByID(userID);
                    //commonResponse.status = Helper.Helper.success_code;
                }
                else if (role == Helper.Helper.doctor)
                {
                    commonResponse.dataenum = _appointementService.GetAppointmentDoctorsByID(userID);
                    commonResponse.status = Helper.Helper.success_code;
                }
                else
                {
                   
                        commonResponse.dataenum = _appointementService.GetAppointmentDoctorsByID(doctorId);
                        commonResponse.status = Helper.Helper.success_code;
                  
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                commonResponse.message = e.Message;
                commonResponse.status = Helper.Helper.failure_code;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("GetCalenderDataById/{id}")]
        public IActionResult GetCalendar(int id)
        {

            CommonResponse<AppointmentVM> commonResponse = new CommonResponse<AppointmentVM>();
            try {
                commonResponse.dataenum = _appointementService.GetById(id);
                commonResponse.status = Helper.Helper.success_code;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                commonResponse.message = e.Message;
                commonResponse.status = Helper.Helper.failure_code;
            }
            return Ok(commonResponse);  
        }

    
    [HttpGet]
    [Route("DeleteAppoinment/{id}")]
    public async Task<IActionResult> DeleteAppoinment(int id)
    {
        CommonResponse<int> commonResponse = new CommonResponse<int>();
        try
        {
            commonResponse.status = await _appointementService.Delete(id);
            commonResponse.message = commonResponse.status == 1 ? Helper.Helper.appointmentDeleted : Helper.Helper.somethingWentWrong;

        }
        catch (Exception e)
        {
            commonResponse.message = e.Message;
            commonResponse.status = Helper.Helper.failure_code;
        }
        return Ok(commonResponse);
    }

    [HttpGet]
    [Route("ConfirmEvent/{id}")]
    public IActionResult ConfirmEvent(int id)
    {
        CommonResponse<int> commonResponse = new CommonResponse<int>();
        try
        {
               

            var result = _appointementService.ConfirmEvent(id).Result;
            if (result > 0)
            {
                commonResponse.status = Helper.Helper.success_code;
               commonResponse.message = Helper.Helper.meetingConfirm;
            }
            else
            {

                commonResponse.status = Helper.Helper.failure_code;
                commonResponse.message = Helper.Helper.meetingConfirmError;
            }

        }
        catch (Exception e)
        {
            commonResponse.message = e.Message;
            commonResponse.status = Helper.Helper.failure_code;
        }
        return Ok(commonResponse);
    }


}
}
