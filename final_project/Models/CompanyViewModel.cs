using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace final_project.Models
{
    public class CompanyViewModel
    {
        public COMPANY Companies { get; set; }
        public IEnumerable<COMPANYFEATURETAB> CompanyFeatuerTabs { get; set; }
    }
}