using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisterAndLogin6.Models
{
    public class Account
    {
        public string Id { get; set; } // PK
        public string UserId { get; set; } // 아이디
        public string Email { get; set; } // 이메일
        public string Password { get; set; } // 비밀번호
    }
}