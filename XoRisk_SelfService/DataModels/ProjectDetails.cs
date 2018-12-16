using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XoRisk_SelfService.DataModels
{
    public class ProjectDetails
    {
        /*
         Id

	Company

	ProjectCode

	StartDate

	EndDate

	Status: ENABLED(1) / DISABLED(0)

	Email

	List<Script>
         */
         public int Id { get; set; }
        public string Company { get; set; }
        public string ProjectCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
        public string POCName { get; set; }
        public string XoRiskEmail { get; set; }
        public List<Scripts> scripts { get; set; }
    }
}