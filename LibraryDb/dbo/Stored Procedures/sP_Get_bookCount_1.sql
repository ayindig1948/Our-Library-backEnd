-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE[dbo].[sP_Get_bookCount]
	-- Add the parameters for the stored procedure here
	@authorId int
	as
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	select count(*) as bookCount from dbo.BookModel b where b.AuthorId=@authorId 
	
END