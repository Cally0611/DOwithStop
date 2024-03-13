using DOwithStop.Data;
using DOwithStop.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DOwithStop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : Controller
    {
        private readonly CustomDBContext _customdbContext;
        private readonly OMplannerDBContext _osmdbContext;
        private readonly IConfiguration _configuration;

        public DetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _customdbContext = new CustomDBContext(_configuration);
            _osmdbContext = new OMplannerDBContext(_configuration);


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
                       .GroupBy(kvp => kvp.MachineID)
                       .ToDictionary(grp => grp.Key, grp => grp.ToList());
            return Json(stopbymachines);
        }

        [HttpGet("Oee")]
        public JsonResult OEEresult()
        {
            List<AllOeeCalculation> oeebyminutes = _customdbContext.AllOeeCalculations.ToList();

            List<AllTargetOee> fixedtargetoees = _customdbContext.AllTargetOees.ToList();

            var oeewithtarget = oeebyminutes.Join(fixedtargetoees, t1 => t1.OeeMachineId, t2 => t2.MachineId, (t1, t2) => new
            {
                oeeMachineId = t1.OeeMachineId,
                oeeInPercentage = t1.OeeInPercentage,
                oeeTarget = t2.TargetOee

            });

            return Json(oeewithtarget);
        }

        [HttpGet("Finishing")]
        public JsonResult FinishingResult()
        {
            List<FinishingAction> finishings = _osmdbContext.FinishingActions.FromSql($"EXECUTE dbo.GetFinishingTgtActual").ToList();

            return Json(finishings);
        }
    }
}
