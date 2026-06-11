CREATE PROCEDURE [dbo].[sP_FulfilBook]
	@bookId int
AS
update dbo.Book set IsFulfiled=1 where Id =@bookId
RETURN 0