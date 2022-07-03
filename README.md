# RegisterAndLogin6
ASP.NET MVC Registration and Login with Dapper
<pre>

</pre>
<hr>
<h2>Table</h2>
CREATE TABLE [dbo].[Account] <br>
(<br>
[Id] [int] NOT NULL,<br>
[Name] [nvarchar](50) NULL,<br>
[Email] [nvarchar](50) NULL,<br>
[Password] [nvarchar](50) NULL,<br>
CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED<br>
)<br>
;
