CREATE TABLE [dbo].[LogApi]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Ctime] [datetime2](6) NOT NULL CONSTRAINT [DF__LogApi__Ctime__32E0915F] DEFAULT (sysdatetime()),
[Createdby] [varchar] (38) COLLATE Chinese_PRC_CI_AS NOT NULL CONSTRAINT [DF__LogApi__Created__33D4B598] DEFAULT ('-1'),
[LogType] [int] NOT NULL,
[LogTypeName] [nvarchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL,
[AccessUrl] [nvarchar] (1024) COLLATE Chinese_PRC_CI_AS NOT NULL,
[IpAddress] [varchar] (15) COLLATE Chinese_PRC_CI_AS NOT NULL,
[ModuleName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL,
[ActionName] [nvarchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL,
[Description] [nvarchar] (4000) COLLATE Chinese_PRC_CI_AS NULL,
[BeforeChange] [nvarchar] (4000) COLLATE Chinese_PRC_CI_AS NULL,
[AfterChange] [nvarchar] (4000) COLLATE Chinese_PRC_CI_AS NULL,
[Remarks] [nvarchar] (500) COLLATE Chinese_PRC_CI_AS NULL
) ON [PRIMARY]
GO
CREATE CLUSTERED INDEX [IX_LogApi_CTIME] ON [dbo].[LogApi] ([Ctime] DESC) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LogApi] ADD CONSTRAINT [PK_LogApi] PRIMARY KEY NONCLUSTERED ([Id]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LogApi_CREATEDBY] ON [dbo].[LogApi] ([Createdby]) ON [PRIMARY]
GO