﻿CREATE TABLE [dbo].[RolesClaim](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_RolesClaim] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--ALTER TABLE [dbo].[RolesClaim]  WITH CHECK ADD  CONSTRAINT [FK_RolesClaim_Role_RoleId] FOREIGN KEY([RoleId])
--REFERENCES [dbo].[Role] ([Id])
--ON DELETE CASCADE
--GO

--ALTER TABLE [dbo].[RolesClaim] CHECK CONSTRAINT [FK_RolesClaim_Role_RoleId]
--GO