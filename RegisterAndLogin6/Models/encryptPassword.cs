using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RegisterAndLogin6.Models
{
    public class encryptPassword
    {
        public static string textToEncrypt(string passWord)
        {
            //ToBase64String : 8비트 부호 없는 정수로 구성된 배열을 base-64 숫자로 인코딩된 해당하는 문자열 표현으로 변환.
            //SHA256 : SHA(Secure Hash Algorithm) 알고리즘의 한 종류로서 256비트로 구성되며 64자리 문자열을 반환. 단방향 암호화 방식이기 때문에 복호화가 불가능.
            //ComputeHash : 지정된 바이트 배열에 대해 해시 값을 계산.
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(passWord)));
        }
    }
}