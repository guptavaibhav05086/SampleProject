﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XoRisk_SelfService.DataModels
{
    public class AdminUser
    {
       public string Username { get; set; }
        public string Email { get; set; }
        public Boolean Active { get; set; }
    }
}