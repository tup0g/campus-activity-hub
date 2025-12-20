using System.Collections.Generic;

namespace CampusActivityHub.ViewModels
{
    public class DashboardViewModel
    {
        public List<string> CategoryLabels { get; set; }
        public List<int> CategoryCounts { get; set; }

        public List<string> EventLabels { get; set; }
        public List<int> EventParticipantsCounts { get; set; }
    }
}