


CREATE TABLE [User](
Id varchar(32) PRIMARY KEY NOT NULL,
CTime datetime2 DEFAULT GETDATE() NOT NULL,
MTime datetime2 DEFAULT GETDATE() NOT NULL,
CreateBy varchar(32) NOT NULL,
ModifyBy varchar(32) NOT NULL,
Name nvarchar(50) NOT NULL,
NickName nvarchar(50) NOT NULL,
Mobilephone varchar(11) NOT NULL,
Email varchar(30) NOT NULL,
Password varchar(200) NOT NULL,
Status int DEFAULT 1 NOT NULL, 
)

CREATE TABLE [UserProfile](
Id varchar(32) PRIMARY KEY NOT NULL,
CTime datetime2 DEFAULT GETDATE() NOT NULL,
MTime datetime2 DEFAULT GETDATE() NOT NULL,
CreateBy varchar(32) NOT NULL,
ModifyBy varchar(32) NOT NULL,
Name nvarchar(50) NOT NULL,
NickName nvarchar(50) NOT NULL,
Mobilephone varchar(11) NOT NULL,
Email varchar(30) NOT NULL,
Country nvarchar(10),
Province nvarchar(10),
City nvarchar(10),
HeadImgUrl nvarchar(32),
Gender int DEFAULT 0 NOT NULL, 
Status int DEFAULT 1 NOT NULL, 
)