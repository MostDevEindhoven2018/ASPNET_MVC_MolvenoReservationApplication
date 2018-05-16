using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public interface ISolutionFinder
    {
        List<List<int>> GetSolutions(List<int> tableCaps, int partySize);
    }
}
