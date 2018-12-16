using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XoRisk_SelfService.DataModels
{
    public class SystemScanReportModel
    {
        public string SystemIP { get; set; }

        public string ProjectCode { get; set; }

        public DateTime? ScanStart { get; set; }

        public DateTime? ScanEnd { get; set; }

        public string ScanStatus { get; set; }

        


    }
}