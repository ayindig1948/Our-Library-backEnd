-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sP_CheckInBook]
	-- Add the parameters for the stored procedure here
	@userId int,
	@bookId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
UPDATE TOP (1) dbo.Book
SET IsCheckedOut = 0,
    UserId = NULL
WHERE UserId = @userId
  AND BookId = @bookId
  AND IsCheckedOut = 1;
  IF @@ROWCOUNT = 1
  begin
  Update dbo.BookModel set NumberOfCopies=NumberOfCopies+1 where dbo.BookModel.Id=@bookId
  end

SELECT @@ROWCOUNT AS BooksReturned;   -- 0 = nothing matched, 1 = checked in
END