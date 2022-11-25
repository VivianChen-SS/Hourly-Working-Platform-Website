using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace final_project.Models
{
    public class AllEventsViewModels
    {
        public string EventID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time_Start { get; set; }
        public TimeSpan? Time_End { get; set; }
        public int NumberOfWorkers { get; set; }
        public string EstimatedCost { get; set; }
        public string ActualCost { get; set; }
        public string statusColor { get; set; }

    }
}