@model IEnumerable<RegisterAndLogin6.Models.SPA.CommentList>
@{
    ViewBag.Title = "Index";
Layout = "~/Views/Shared/_Layout.cshtml";

    @*RegisterAndLogin6.Models.SPA.CommentList commentList = ViewBag.comment as RegisterAndLogin6.Models.SPA.CommentList;

RegisterAndLogin6.Models.SPA.ReplyList replyList = ViewBag.reply as RegisterAndLogin6.Models.SPA.ReplyList; *@
}

< h2 > Single Page Application</h2>
   <script src = "~/Scripts/jquery-3.6.0.min.js" ></ script >
   

   < hr />
   

   < input type= "text" class= "txt_Comment" />
     < input type = "button" id = "btn_save" value = "저장" />
          

          < table class= "table" >
           
               < thead >
           
                   < tr >
           
                       < th > 번호 </ th >
           
                       < th > 글내용 </ th >
           
                       < th > 작성자 </ th >
           
                       < th > 작성일 </ th >
           
                   </ tr >
           
               </ thead >
           
               < tbody >
                   @foreach(var item in Model)
        {
            < tr >
                < td > @item.Id </ td >
                @*< td >< a href = "#" onclick = "return editComment(@item.Id, '@item.Comment')" > @item.Id </ a ></ td > *@
                < td > @item.Comment </ td >
                @*< td >< a class= "data_Comment" href = "#" data - Id = "@item.Id" > @item.Comment </ a ></ td > *@
                @*< td > @item.UserId </ td > *@
                < td >
                    < a href = "#" onclick = "return mention(@item.Id)" > @item.UserId </ a >
   
                   </ td >
   
                   < td > @item.Regdate </ td >
   
                   < td >
   
                       < a href = "#" onclick = "return editComment(@item.Id, '@item.Comment')" > 수정 </ a > |
                    @*< a href = "#" id = "btn_delete" onclick = "return deleteComment()" > 삭제 </ a > *@
                    < a href = "#" id = "btn_delete" > 삭제 </ a >
                    @*< input type = "button" id = "btn_delete" value = "삭제" /> *@
                </ td >
            </ tr >
        }
    </ tbody >
</ table >


< !--게시글 영역-- >
@*< div class= "Comment" id = "Comment" >
  

  </ div > *@
< !--게시글 영역-- >
< !--댓글 영역-- >
@*< div class= "Reply" id = "Reply" >
  
      < button type = "button" class= "btn btn-primary" onclick = "ReplySubmit()" value = "Login" id = "btnLogin" > 등록 </ button >
          </ div > *@
< !--// 댓글 영역 -->

< script >
    function editComment(_Id, _Comment) {
        $(".hid_Id").val(_Id);
        $(".txt_Comment").val(_Comment);
    return false;
};

    @*function deleteComment() {
    //$(".txt_Comment").val("");
    return false;
}; *@

    function mention()
{
        $(".txt_Comment").val("멘션기능추가");
        $(".txt_Comment").focus();
    return false;
}

    $(document).ready(() => {
        $("#btn_save").on("click", function() {
            $.ajax({
         method: "POST",
                url: "/SPA/Update",
                data: { Id: ($(".txt_Comment").data("Id") == undefined ? 0 : $(".txt_Comment").data("Id")), Comment: $(".txt_Comment").val() },
                success: function() {
    alert("저장완료!!");
    window.location.reload();
}
            })
        });

        $("#btn_delete").on("click", function() {
            //var Id = $("#btn_delete").val();
            $.ajax({
    method: "POST",
                url: "/SPA/Delete",
                data: { Id = Id },
                success: function() {
            alert("삭제 완료!!");
            window.location.reload();
        }
    })
        });

        $("a.data_Comment").on("click", function() {
            $(".txt_Comment").data("Id", $(this).data("Id"))
            $(".txt_Comment").val($(this).text())
            return false;
});

    });

    @*let page = 1
    let pagesize = 10

    $(document).ready(function() {
    CommentList();
    //ReplyList();
})

    var CommentSubmit = function() {
        var Comment = $("#Comment").val();
        $.ajax({
type: "POST",
            url: "/SPA/CommentSubmit",
            data:
    {
    Comment: Comment
            },
            success: function() {
        alert("게시글이 등록되었습니다.");
        CommentList();
    }
})
    }

    function CommentList()
{
    let _commentList = $(".Comment");
    for (let i = 0; i < pagesize; i++)
    {
            $(_commentList).append(addCommentList())
        }

    let _data = {
            page: page,
            pagesize: pagesize
        }

        $.ajax({
type: "POST",
            url: "@Url.Action("CommentList", "SPA", new {Id = commentList.Id})",
            data: _data,
            success: function(data) {
        let _commentList = $(".Comment");
        for (let i = 0; i < data.length; i++)
        {

        }
    }
})

    }


    var ReplySubmit = function() {
        var Reply = $("#Reply").val();
        $.ajax({
type: "POST",
            url: "/SPA/ReplySubmit",
            data:
    {
    Reply: Reply
            },
            success: function() {
        alert("댓글이 등록되었습니다.");
        ReplyList();
    }
})
    }

    function ReplyList()
{
    let _replyList = $(".Reply");
    for (let i = 0; i < pagesize; i++)
    {
            $(_replyList).append(addReplyList())
        }

    let _data = {
            page: page,
            pagesize: pagesize
        }

        $.ajax({
type: "POST",
            url: "@Url.Action("ReplyList", "SPA", new {Id = Id})",
            data: _data,
            success: function(data) {
        let _replyList = $(".Reply");
        for (let i = 0; i < data.length; i++)
        {

        }
    }
})

    }*@
</ script >