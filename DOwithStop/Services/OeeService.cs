using DOwithStop.Data;
using DOwithStop.Interfaces;
using DOwithStop.Models;

namespace DOwithStop.Services
{
    public class OeeService : IOee
    {
       public List<AllTargetOee> GetTargetOee(CustomDBContext customDBContext)
        {
            return customDBContext.AllTargetOees.ToList();
        }
    }
}   
