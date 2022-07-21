# RegisterAndLogin6
ASP.NET MVC 5 Board with Dapper
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
