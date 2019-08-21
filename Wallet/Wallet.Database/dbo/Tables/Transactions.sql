CREATE TABLE [dbo].[Transactions] (
    [TransactionId] UNIQUEIDENTIFIER NOT NULL,
    [FromUserId]    INT              NOT NULL,
    [ToUserId]      INT              NOT NULL,
    [Amount]        DECIMAL (18, 2)  NOT NULL,
    [Purpose]       NVARCHAR (300)   NULL,
    [TransactionAt] DATETIME         NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([TransactionId] ASC),
    CONSTRAINT [FK_Transactions_Users_From] FOREIGN KEY ([FromUserId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_Transactions_Users_To] FOREIGN KEY ([ToUserId]) REFERENCES [dbo].[Users] ([UserId])
);

