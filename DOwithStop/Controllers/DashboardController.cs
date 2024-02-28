using DOwithStop.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DOwithStop.Controllers
{
   
    public class DashboardController : Controller
    {
   

       
        public IActionResult Index()
        {
            Log.Error("Starting Controller");
            return View();
        }

        public IActionResult Index4K()
        {
            Log.Error("Starting Controller");
            return View();
        }


    }
}
