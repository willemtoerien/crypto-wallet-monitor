CREATE TABLE [dbo].[Users] (
    [UserId]   INT             IDENTITY (1, 1) NOT NULL,
    [Email]    NVARCHAR (300)  NOT NULL,
    [Password] NVARCHAR (100)  NOT NULL,
    [Balance]  DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

