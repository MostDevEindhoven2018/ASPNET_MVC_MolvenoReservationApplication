using ASPNET_MVC_MolvenoReservationApplication.Models;
using System.Collections.Generic;

namespace ASPNET_MVC_MolvenoReservationApplication.Logic
{
    public interface ISolutionScorer
    {
       List<int> GetBestTableConfiguration(List<List<int>> ViableSolutions, int PartySize);
    }
}