using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace final_project.Models
{
    public class EventChooseTabViewModel
    {
        [StringLength(20, ErrorMessage = "請勿輸入過長的標籤。")]
        public string SkillTabSearchString { get; set; }

        [StringLength(20, ErrorMessage = "請勿輸入過長的標籤。")]
        public string WorkTabSearchString { get; set; }

        //public Dictionary<REQUIREDSKILLTAB,bool> RequiredSkillTabs { get; set; }
    }
}