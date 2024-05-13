using DOwithStop.Data;
using DOwithStop.Models;

namespace DOwithStop.Interfaces
{
    public interface ITarget
    {
        LastShift GetSummaryShiftTargetDtl (CustomDBContext customDBContext);
    }
}
