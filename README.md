# RegisterAndLogin6
ASP.NET MVC 5 Board with Dapper and Stored Procedures
<pre>

</pre>
<hr>
<h2>Table</h2>
CREATE TABLE [dbo].[Account] <br>
(<br>
  [Id] [int] IDENTITY(1,1) NOT NULL,<br>
  [UserId] [nvarchar](50) NULL,<br>
  [Email] [nvarchar](50) NULL,<br>
  [Password] [nvarchar](50) NULL,<br>
  [EmailVerification] [bit] NULL,<br>
  ActivationCode [uniqueidentifier] NULL<br>
  CONSTRAINT [PK_Account_1] PRIMARY KEY CLUSTERED<br>
)<br>
;
<pre>

</pre>
CREATE TABLE [dbo].[Board]<br>
(<br>
	[Id] [int] IDENTITY(1,1) NOT NULL,<br>
	[Title] [nvarchar](50) NULL,<br>
	[Comment] [nvarchar](500) NULL,<br>
	[UserId] [nvarchar](50) NULL,<br>
	[Regdate] [datetime] NULL,<br>
  	CONSTRAINT [PK_Board_1] PRIMARY KEY CLUSTERED <br>
 )<br>
 ;
<pre>

</pre>
CREATE TABLE [dbo].[CommentList]<br>
(<br>
	[Id] [int] IDENTITY(1,1) NOT NULL,<br>
	[ParentId] [nvarchar](50) NULL,<br>
	[Lv] int NULL,<br>
	[Comment] [nvarchar](500) NULL,<br>
	[Image] [nvarchar](500) NULL,<br>
	[UserId] [nvarchar](50) NULL,<br>
	[Regdate] [datetime] NULL,<br>
  	CONSTRAINT [PK_Board_1] PRIMARY KEY CLUSTERED <br>
)<br>
;
<pre>

</pre>
<hr>
<h2>CTE 계층형 트리구조 재귀함수 (Common Table Expression in SQL Server)</h2>
WITH CTE AS<br>
(<br>
	SELECT <br>
		Id, <br>
		Comment,<br>
		ParentId, <br>
		Lv,<br>
		CONVERT(NVARCHAR(100), Id) Depth,<br>
		UserId, <br>
		Regdate<br>
	FROM CommentList<br>
	WHERE ParentId IS NULL<br>
<br>
	UNION ALL<br>
<br>
	SELECT <br>
		b.Id, <br>
		b.Comment, <br>
		b.ParentId, <br>
		b.Lv, <br>
		CONVERT(NVARCHAR(100), CONCAT(c.Depth, ',', b.Id)) Depth,<br>
		b.UserId, <br>
		b.Regdate<br>
	FROM CommentList b<br>
	INNER JOIN CTE c<br>
	ON b.ParentId = c.Id<br>
)<br>
<br>
SELECT <br>
	Id, <br>
	ParentId, <br>
	CONCAT(REPLICATE('		', Lv), 'ㄴ', Comment) AS Comment, <br>
	Lv, <br>
	Depth,<br>
	UserId,<br>
	Regdate<br>
FROM CTE<br>
ORDER BY Depth<br>
;<br>
<pre>

</pre>
<hr>
<h2>CommentList 관련 Stored Procedures</h2>
Create Procedure [dbo].[sp_SPA_CommentList_Select]  <br>
	(<br>
		@Id			int,<br>
		@ParentId	nvarchar(50),<br>
		@TopId		int,<br>
		@Lv			int,<br>
		@Comment	nvarchar(500),<br>
		@UserId		nvarchar(50),<br>
		@Image		nvarchar(500),<br>
		@Regdate	datetime<br>
	)<br>
as     <br>
Begin    <br>
	SELECT <br>
		Id, ParentId, TopId, Lv, Comment, Image, UserId, Regdate<br>
	FROM <br>
		CommentList<br>
	(nolock)<br>
	ORDER BY Id DESC; <br>
End  <br>
;<br>
<hr>
Create Procedure [dbo].[sp_SPA_CommentList_Insert] <br>
	(<br>
	 @Id		int,<br>
	 @ParentId	nvarchar(50),<br>
	 @TopId		int,<br>
	 @Lv		int,<br>
	 @UserId	nvarchar(50),<br>
	 @Comment	nvarchar(500),<br>
	 @Image		nvarchar(500)<br>
	)<br>
as     <br>
Begin    <br>
	INSERT INTO CommentList <br>
		(ParentId, TopId, Lv, Comment, UserId, Image, Regdate) <br>
	Values <br>
		(@ParentId, @TopId, @Lv, @Comment, @UserId, @Image, Getdate())<br>
End  <br>
;<br>
<hr>
Create Procedure [dbo].[sp_SPA_CommentList_Update] <br>
	(<br>
	 @Id		int,<br>
	 @Comment	nvarchar(500),<br>
	 @Image		nvarchar(500)<br>
	)<br>
as     <br>
Begin    <br>
	Update <br>
		CommentList <br>
	Set<br>
		Comment = @Comment,<br>
		Image = @Image,<br>
		Regdate = GETDATE()<br>
	Where<br>
		Id = @Id<br>
End  <br>
;<br>
<hr>
Create Procedure [dbo].[sp_SPA_CommentList_Delete] <br>
	@Id	int<br>
as     <br>
Begin    <br>
	Delete From <br>
		CommentList <br>
	Where<br>
		Id = @Id OR TopId = @Id<br>
End  <br>
;<br>
