using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisterAndLogin6.Models
{
    public class Board
    {
        public int Id { get; set; } //PK
        public string Title { get; set; } //제목
        public string Comment { get; set; } //댓글 내용
        public string UserId { get; set; } //작성자
        public DateTime Regdate { get; set; } //작성일
    }
}