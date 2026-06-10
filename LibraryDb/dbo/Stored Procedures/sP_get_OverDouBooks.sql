-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sP_get_OverDouBooks]
	-- Add the parameters for the stored procedure here
	@userId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECt b.Id ,bm.Title,b.DueDate ,b.IsCheckedOut,b.UserId from dbo .Book b inner join dbo.BookModel bm on b.BookId=bm.Id where b.UserId=@userId and DueDate<GetDate()
END