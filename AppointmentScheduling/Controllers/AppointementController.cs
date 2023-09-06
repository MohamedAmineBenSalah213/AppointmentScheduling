using AppointementScheduling.Helper;
using AppointementScheduling.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointementScheduling.Controllers
{
   
    public class AppointementController : Controller
    {
        private readonly IAppointementService _appointementService;
        public AppointementController(IAppointementService appointementService)
        {
            _appointementService = appointementService;
        }
        [Authorize(Roles = Helper.Helper.admin)]

        public IActionResult Index()
        {ViewBag.Duration=Helper.Helper.GetTimeDropDown();
          ViewBag.Drop=  _appointementService.GetDoctorList();
            ViewBag.DropPatient = _appointementService.GetPatientList();
            return View();
        }

    }
}
