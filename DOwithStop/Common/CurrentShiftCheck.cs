using DOwithStop.Models;

namespace DOwithStop.Common
{
    public static class CurrentShiftCheck
    {
        public static CurrentShift CheckCurrentShiftTime(DateTime currdt)
        {
            TimeSpan timeSpan = currdt.TimeOfDay;
            TimeSpan shift1 = new TimeSpan(7, 0, 0);
            TimeSpan shift2 = new TimeSpan(19, 0, 0);

            CurrentShift currShift = new CurrentShift();


            if (timeSpan >= shift1 && timeSpan < shift2)
            {
                currShift.ShiftStart = currdt.Date.Add(shift1);
                currShift.ShiftEnd = currdt.Date.Add(shift2);
                currShift.CurrentShiftNo = 1;
            }
            else if (timeSpan >= shift2)
            {
                currShift.ShiftStart = currdt.Date.Add(shift2);
                currShift.ShiftEnd = currdt.Date.AddDays(1);
                currShift.CurrentShiftNo = 2;
            }
            else if (timeSpan < shift1)
            {
                currShift.ShiftStart = currdt.Date.AddDays(-1).Add(shift2);
                currShift.ShiftEnd = currdt.Date.Add(shift1);
                currShift.CurrentShiftNo = 2;
            }

            return currShift;

        }
    }
}
