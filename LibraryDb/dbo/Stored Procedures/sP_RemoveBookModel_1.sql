CREATE PROCEDURE [dbo].[sP_RemoveBookModel]
	@Id int
AS
	delete from BookModel where Id = @Id 
	delete from Book where BookId = @Id