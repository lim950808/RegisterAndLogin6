using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegisterAndLogin6.Models
{
    public class Account
    {
        public int Id { get; set; } //PK

        public string UserId { get; set; } //아이디

        public string Email { get; set; } //이메일

        public string Password { get; set; } //비밀번호 (단방향 암호화)

        public bool? EmailVerification { get; set; } //회원가입시 이메일 인증 여부

        public Guid? ActivationCode { get; set; } //암호화된 코드번호

        public string OTP { get; set; } //OTP 인증코드 4자리

        public bool RememberMe { get; set; } //Remember me?
    }
}