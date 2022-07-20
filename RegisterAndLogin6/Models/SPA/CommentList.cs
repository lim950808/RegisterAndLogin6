using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisterAndLogin6.Models.SPA
{
    public class CommentList
    {
        public int Id { get; set; } //PK

        public string UserId { get; set; } //작성자

        public string Comment { get; set; } //게시글

        //public string Picture { get; set; } //사진

        public DateTime Regdate { get; set; } //등록일자
    }
}