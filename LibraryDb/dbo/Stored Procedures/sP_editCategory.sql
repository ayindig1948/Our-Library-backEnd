CREATE PROCEDURE [dbo].[sP_editCategory]

	@category nvarchar(50),
	@bookId int
AS
	update dbo.BookModel set Category =@category where Id=@bookId
RETURN 0