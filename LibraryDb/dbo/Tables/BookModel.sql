CREATE TABLE [dbo].[BookModel] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (50) NOT NULL,
    [Description]    NCHAR (280)   NOT NULL,
    [Category]       NVARCHAR (50) NOT NULL,
    [AuthorId]       INT           NOT NULL,
    [NumberOfCopies] INT           NULL,
    CONSTRAINT [PK_BookModel] PRIMARY KEY CLUSTERED ([Id] ASC)
);

