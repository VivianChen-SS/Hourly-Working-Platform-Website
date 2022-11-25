using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace final_project.Models
{
    public class ResumeListViewModels
    {
        public string WorkerID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string School { get; set; }
        public string GraduationStatus { get; set; }
        public List<string> SkillTabs { get; set; }
        public bool Hire { get; set; }
        //public Dictionary<string,string> Experience { get; set; }
        public int Rating { get; set; }
        public bool hasPhoto { get; set; }
    }
}