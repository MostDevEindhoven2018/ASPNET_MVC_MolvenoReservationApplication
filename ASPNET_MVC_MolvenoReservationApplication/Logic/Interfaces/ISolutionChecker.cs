using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNET_MVC_MolvenoReservationApplication.Models;


namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public interface ISolutionChecker
    {
        List<List<int>> GetViableSolutions(List<List<int>> AllSolutions, Dictionary<int, int> tableCapAmounts);
    }
}
