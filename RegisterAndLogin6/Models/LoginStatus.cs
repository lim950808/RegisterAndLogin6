using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisterAndLogin6.Models
{
    public class LoginStatus
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string TargetURL { get; set; }
    }
}