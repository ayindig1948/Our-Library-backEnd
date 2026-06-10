
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sP_AddAuther]
	@firstName nvarchar(50),
	@lastName nvarchar(50)
	as
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into dbo.Auther (FirstName, LastName)values(@firstName,@lastName)
	select SCOPE_IDENTITY() as 'AutherID'
END