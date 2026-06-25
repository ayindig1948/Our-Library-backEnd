CREATE PROCEDURE [dbo].[sP_editDescription]
	@description nvarchar(250),
	@bookId int
AS
	update dbo.BookModel set Description =@description where Id=@bookId
RETURN 0
