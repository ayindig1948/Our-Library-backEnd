-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [sP_GetBookId]
	-- Add the parameters for the stored procedure here
	@authorId int,
	@titel nvarchar(50)
	as
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
select Id from dbo.BookModel where Title=@titel and AuthorId=@authorId
END
