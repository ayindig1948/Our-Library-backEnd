CREATE PROCEDURE [dbo].[sP_get_AllCheckedOutBooks]
    @userId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.Id,
        bm.Title,
        b.DueDate
      
     
    FROM dbo.Book b
    INNER JOIN dbo.BookModel bm ON b.BookId = bm.Id
    WHERE b.UserId = @userId AND b.IsCheckedOut = 1	
END