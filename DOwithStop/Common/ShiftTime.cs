using DOwithStop.Models;

namespace DOwithStop.Common
{
    public static class ShiftTime
    {
        public  static LastShift CheckLastShiftTime(DateTime currdt)
        {
            TimeSpan timeSpan = currdt.TimeOfDay;
            TimeSpan shift1 = new TimeSpan(7, 0, 0);
            TimeSpan shift2 = new TimeSpan(19, 0, 0);

            LastShift lastShift = new LastShift();


            if (timeSpan >= shift1 && timeSpan < shift2)
            {
                lastShift.LastShiftDate = currdt.Date.AddDays(-1); 
                lastShift.LastShiftNo = 2;
                lastShift.ShiftStartDT = currdt.Date.AddDays(-1).Add(shift2);
                lastShift.ShiftEndDT = currdt.Date.Add(shift1);
            }
            else if (timeSpan >= shift2)
            {
                lastShift.LastShiftDate = currdt.Date;
                lastShift.LastShiftNo = 1;
                lastShift.ShiftStartDT = currdt.Date.Add(shift1);
                lastShift.ShiftEndDT = currdt.Date.Add(shift2);
            }
            else if (timeSpan < shift1)
            {
                lastShift.LastShiftDate = currdt.Date.AddDays(-1);
                lastShift.LastShiftNo = 1;
                lastShift.ShiftStartDT = currdt.Date.AddDays(-1).Add(shift1);
                lastShift.ShiftEndDT = currdt.Date.AddDays(-1).Add(shift2);
            }

            return lastShift;

        }
    }
}
