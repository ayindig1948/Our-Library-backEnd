CREATE PROCEDURE [dbo].[sP_GetBooksToFulfil]

AS
SELECt b.Id ,bm.Title,b.DueDate ,b.IsCheckedOut,b.UserId from dbo .Book b inner join dbo.BookModel bm on b.BookId=bm.Id where IsCheckedOut=1 and IsFulfiled=0