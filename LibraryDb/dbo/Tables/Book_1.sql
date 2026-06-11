CREATE TABLE [dbo].[Book] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [AuthorId]     INT           NULL,
    [UserId]       INT           NULL,
    [IsCheckedOut] BIT           CONSTRAINT [DF_Book_isChackedOut] DEFAULT ((0)) NOT NULL,
    [DueDate]      DATETIME2 (7) NULL,
    [BookId]       INT           NOT NULL,
    [IsFulfiled]   BIT           CONSTRAINT [DF_Book_isFulfiled] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED ([Id] ASC)
);

