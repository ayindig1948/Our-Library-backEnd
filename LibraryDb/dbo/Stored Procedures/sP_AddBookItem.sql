-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sP_AddBookItem]
	-- Add the parameters for the stored procedure here
	@autherId int,
	@bookId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into dbo.Book(AuthorId,BookId)values(@autherId,@bookId)
	update dbo.BookModel set NumberOfCopies = NumberOfCopies + 1 where BookModel.Id=@bookId
	end