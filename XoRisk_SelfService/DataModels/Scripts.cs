using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XoRisk_SelfService.DataModels
{
    public class Scripts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MappedFolderPath { get; set; }
       
        public bool Status { get; set; }

        public string ImagePath { get; set; }


    }
}