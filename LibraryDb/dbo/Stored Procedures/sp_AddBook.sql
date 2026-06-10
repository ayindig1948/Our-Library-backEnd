-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_AddBook]
	-- Add the parameters for the stored procedure here
	@title nvarchar (50),
	@description nvarchar(280),
	@category nvarchar(50),
	@autherId int,
	@numberOfCopies int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into dbo.BookModel (Title,[Description],Category,AuthorId,NumberOfCopies) values (@title,@description,@category,@autherId,@numberOfCopies)
	SELECT SCOPE_IDENTITY() AS NewBookID;
END