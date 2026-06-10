CREATE TABLE [dbo].[Auther] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (50) NOT NULL,
    [LastName]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Auther] PRIMARY KEY CLUSTERED ([Id] ASC)
);

