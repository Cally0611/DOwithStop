using DOwithStop.Data;
using DOwithStop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOwithStop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : Controller
    {
        private readonly CustomDBContext _customdbContext;
        private readonly IConfiguration _configuration;

        public DetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _customdbContext = new CustomDBContext(_configuration);

        }

        [HttpGet("TargetActual")]
        public JsonResult TargetActual()
        {
            //List<AllMachine> x = _customdbContext.AllMachines.FromSql($"EXECUTE dbo.GetCurrTgtvsActBatch {"2024-01-15"}").ToList();
            List<AllMachine> x = _customdbContext.AllMachines.FromSql($"EXECUTE dbo.GetCurrTgtvsActBatch").ToList();
            return Json(x);
        }

        [HttpGet("StopReasons")]
        public JsonResult StopReasons()
        {
            List<StopReason> allreasons = _customdbContext.StopReasons.FromSqlRaw($"EXECUTE dbo.GetStopDetailsByShift").ToList();
            

            Dictionary<int, List<StopReason>> stopbymachines = allreasons
                       .GroupBy(kvp => kvp.OeeMachine)
                       .ToDictionary(grp => grp.Key, grp => grp.ToList());
            return Json(stopbymachines);
        }
    }
}
