CREATE TABLE [dbo].[User] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (50) NULL,
    [LastName]    NVARCHAR (50) NULL,
    [UserSubname] NVARCHAR (50) NOT NULL,
    [IsMember]    BIT           CONSTRAINT [DF_User_IsMember] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);

