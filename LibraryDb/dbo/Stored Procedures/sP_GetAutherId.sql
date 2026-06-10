-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sP_GetAutherId]
	-- Add the parameters for the stored procedure here
	@firstName nvarchar(50),
	@lastName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT id from dbo.Auther where FirstName=@firstName and LastName=@lastName
END
