using DOwithStop.Common;
using DOwithStop.Data;
using DOwithStop.Interfaces;
using DOwithStop.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NuGet.Packaging.Signing;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace DOwithStop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : Controller
    {
        //DI as property
        private readonly CustomDBContext _customdbContext;
        private readonly OMplannerDBContext _osmdbContext;
        private readonly SQLwoEFhelper _sqlwoEFhelper;
        private readonly IConfiguration _configuration;
        private readonly IOee _oeetargets;
        
        public DetailsController(IConfiguration configuration, IOee oeetargets)
        {
            _configuration = configuration;
            _customdbContext = new CustomDBContext(_configuration);
            _osmdbContext = new OMplannerDBContext(_configuration);
            _sqlwoEFhelper = new SQLwoEFhelper(_customdbContext);
            _oeetargets = oeetargets;
        }

        [HttpGet("TargetActual")]
        public JsonResult TargetActual()
        {
            List<ChildAllMachine> ls = _sqlwoEFhelper.ExecuteSPsql<ChildAllMachine>("GetCurrTgtvsActBatch");
            return Json(ls);
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

        [HttpGet("LastShiftStopReasons")]
        public JsonResult LastShiftStopReasons()
        {
            List<StopReason> alllstreasons = _customdbContext.StopReasons.FromSqlRaw($"EXECUTE dbo.GetStopDetailsByLstShift").ToList();

            Dictionary<int, List<StopReason>> stopbylastmachines = alllstreasons
                       .GroupBy(kvp => kvp.MachineID)
                       .ToDictionary(grp => grp.Key, grp => grp.ToList());
            return Json(stopbylastmachines);
        }


        [HttpGet("Oee")]
        public JsonResult OEEresult()
        {
            List<AllOeeCalculation> oeebyminutes = _customdbContext.AllOeeCalculations.ToList();
            oeebyminutes = oeebyminutes
                .Where(x => x.OeeMachineId != 3).ToList();

            List<AllTargetOee> fixedtargetoees = _oeetargets.GetTargetOee(_customdbContext).ToList();
            fixedtargetoees = fixedtargetoees
                            .Where(x => x.MachineId != 3|| x.MachineId != 17 || x.MachineId != 18).ToList();

            var oeewithtarget = oeebyminutes.Join(fixedtargetoees, t1 => t1.OeeMachineId, t2 => t2.MachineId, (t1, t2) => new
            {
                oeeMachineId = t1.OeeMachineId,
                oeeInPercentage = t1.OeeInPercentage,
                oeeTarget = t2.TargetOee

            });

            return Json(oeewithtarget);
        }


        [HttpGet("LastShiftOee")]
        public JsonResult LastShiftOeeresult(int parammchID)
        {
            LastShift lsa = ShiftTime.CheckLastShiftTime(DateTime.Now);


            List<AllTargetOee> targetoee = _oeetargets.GetTargetOee(_customdbContext)
                .Where(x => x.MachineId == parammchID)
                .ToList();

            List<AllOee> oeeitem = _customdbContext.AllOees
                .Where(x => x.MachineId == parammchID)
                .Where(y => y.OeeDateTime >= lsa.ShiftStartDT && y.OeeDateTime < lsa.ShiftEndDT)
                .OrderByDescending(x => x.OeeDateTime)
                .Take(1)
                .ToList();

            var xy = oeeitem.Join(targetoee,
                x => x.MachineId,
                y => y.MachineId,
                  (x, y) => new OEE()
                  {
                      OeeMachineId = x.MachineId,
                      OeeAvailability = (Convert.ToDecimal(x.OeeUptime)/ 720)*100,
                      OeePerformance = (x.OeeTotalSheets / x.OeeUptime / y.MaxSheetsPerMin)*100,
                      OeeTarget = y.TargetOee
            });

            return Json(xy);
        }


        [HttpGet("Finishing")]
        public JsonResult FinishingResult()
        {
            List<FinishingAction> finishings = _osmdbContext.FinishingActions.FromSql($"EXECUTE dbo.GetFinishingTgtActual").ToList();

            return Json(finishings);
        }


        [HttpGet("FinishingReason")]
        public JsonResult FinishingReason(int paramfinID)
        {
            CurrentShift cs = CurrentShiftCheck.CheckCurrentShiftTime(DateTime.Now);

            List<FinishingReasonUser> finishreason_plant = _osmdbContext.FinishingReasonUsers
                .Where(x => x.FinishrsnPlant == paramfinID)
                .Where(x => x.FinishrsnTimeStamp >= cs.ShiftStart && x.FinishrsnTimeStamp < cs.ShiftEnd)
                .OrderByDescending(x => x.FinishrsnTimeStamp)
                .Take(1)
                .ToList();


            if(finishreason_plant.Count() > 0)
            {
                var xnresult = finishreason_plant.Join(_osmdbContext.FinishingReasons,
                                  x => x.FinishrsnText,
                                  y => y.FinishSRID,
                                  (x, y) => new
                                  {
                                      FinishrsnID = x.FinishrsnID,
                                      FinishrsnText = x.FinishrsnText,
                                      FinishrsnPlainText = y.FinishReason,
                                      FinishrsnPlant = x.FinishrsnPlant,
                                      FinishrsnTimeStamp = x.FinishrsnTimeStamp
                                  });
                return Json(xnresult);
            }
            else
            {
                return Json("No Reason Selected");

            }
        

         
        }


        [HttpGet("LastShiftTarget")]
        public JsonResult LastShiftTarget(int parammchID)
        {
            LastShift ls = ShiftTime.CheckLastShiftTime(DateTime.Now);

            //var ty = _customdbContext.AllMachines
            //    .Where(x => x.TargetDate == ls.LastShiftDate.Date)
            //    .Where(y => y.MachineID == parammchID)
            //    .Select(o =>
            //            new
            //            {
            //                MachineID = parammchID,
            //                JobName = o.JobName1,
            //                TargetDate = o.TargetDate,
            //                PReasonCode = ls.LastShiftNo == 1 ? o.ParentReasonCodeShift1 : o.ParentReasonCodeShift2,
            //                CReasonCode = ls.LastShiftNo == 1 ? o.ChildReasonCodeShift1 : o.ChildReasonCodeShift2
            //            }).ToList();

            List<LastShift> lst = _customdbContext.AllMachines
                .Where(x => x.TargetDate == ls.LastShiftDate.Date)
                .Where(y => y.MachineID == parammchID)
                .Select(o =>
                        new LastShift()
                        {
                            MachineID = parammchID,
                            JobName = o.JobName1,
                            LastShiftDate = o.TargetDate,
                            PReasonCode = Convert.ToInt32(ls.LastShiftNo == 1 ? o.ParentReasonCodeShift1 : o.ParentReasonCodeShift2),
                            CReasonCode = Convert.ToInt32(ls.LastShiftNo == 1 ? o.ChildReasonCodeShift1 : o.ChildReasonCodeShift2)
                        }).ToList();

            return Json(lst);

        }
    }
}
