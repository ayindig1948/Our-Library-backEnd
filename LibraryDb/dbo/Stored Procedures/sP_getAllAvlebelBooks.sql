-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE[sP_getAllAvlebelBooks]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select bm.*,a.*from dbo.BookModel bm inner  join dbo.Book b on b.BookId=bm.Id inner join dbo.Auther a on bm.AuthorId=a.Id where b.IsCheckedOut=0
END