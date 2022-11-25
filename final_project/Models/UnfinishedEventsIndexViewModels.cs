using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace final_project.Models
{
    public class UnfinishedEventsIndexViewModels
    {
        public string EventID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime OfflistTime { get; set; }
        public TimeSpan Time_Start { get; set; }
        public TimeSpan? Time_End { get; set; }
        public int OffListHoursLeft { get; set; }
        public int WorkDayHoursLeft { get; set; }
        public int NumberOfWorkers { get; set; }
        public int NumberOfHired { get; set; }
        public int StillNeed { get; set; }
        public double HeatColorPercentage { get; set; }
    }
}