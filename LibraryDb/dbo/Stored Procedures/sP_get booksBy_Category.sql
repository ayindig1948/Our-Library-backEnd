-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sP_get booksBy_Category]
	-- Add the parameters for the stored procedure here
	@category nvarchar (50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT bm .*,a.*from dbo.BookModel bm inner join dbo.Auther a on bm.AuthorId=a.Id where Category=@category
END