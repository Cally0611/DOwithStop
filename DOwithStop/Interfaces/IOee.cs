using DOwithStop.Data;
using DOwithStop.Models;

namespace DOwithStop.Interfaces
{
    public interface IOee
    {
        List<AllTargetOee> GetTargetOee(CustomDBContext customDBContext);
    }
}
