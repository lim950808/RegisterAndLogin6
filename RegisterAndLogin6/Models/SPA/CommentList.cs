using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisterAndLogin6.Models.SPA
{
    public class CommentList
    {
        public int Id { get; set; } //PK

        public string ParentId { get; set; } //부모 상위 트리 Id

        public int TopId { get; set; } //최상위 트리 Id

        public int Lv { get; set; } //트리 계층 Level

        public string UserId { get; set; } //작성자

        public string Comment { get; set; } //게시글

        public string Image { get; set; } //사진

        public DateTime Regdate { get; set; } //등록일자
    }
}