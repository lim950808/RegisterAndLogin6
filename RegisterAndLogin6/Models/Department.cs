using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisterAndLogin6.Models
{
    public class Department
    {
        public string Id { get; set; } //PK
        public string DepName { get; set; } //부서명
        public string Job { get; set; } //직위
    }
}