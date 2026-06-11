CREATE PROCEDURE [dbo].[sP_getAvlebelBooksByCategory]
	@category NVARCHAR(50)
AS
	select bm.*,a.*from dbo.BookModel bm inner  join dbo.Book b on b.BookId=bm.Id inner join dbo.Auther a on bm.AuthorId=a.Id where b.IsCheckedOut=0 and bm.Category=@category