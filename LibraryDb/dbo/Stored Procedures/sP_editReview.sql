CREATE PROCEDURE [dbo].[sP_editDescription]
	@description nvarchar(250),
	@boookId int
AS
	update dbo.BookModel set Description =@description where Id=@boookId
RETURN 0
