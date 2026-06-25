CREATE PROCEDURE [dbo].[sP_editTitle]
	@title nvarchar(50),
	@bookId int
AS
	update dbo.BookModel set Title =@title where Id=@bookId
RETURN 0