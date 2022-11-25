using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace final_project.Models
{
    public class MBITPreferenceViewModels
    {
        [Required]
        public string attitudes { get; set; }
        [Required]
        public string functions_SN { get; set; }
        [Required]
        public string functions_TF { get; set; }
        [Required]
        public string lifestylePreferences { get; set; }
        [Required]
        public string RequiredCharacter { get; set; }

    }
}