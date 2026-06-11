-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sP_GetUserId] 
   
    @sub Nvarchar (50)
   -- @userid INT OUTPUT -- Added output parameter
AS 
BEGIN 
    SET NOCOUNT ON; 
	 -- Local variable to hold the user ID	
    -- Attempt to find the ID first
	declare @userid INT;
    SELECT @userid =Id
    FROM dbo.[user] 
    where UserSubname=@sub
   
 

    -- If not found, insert and grab the new ID
    IF @userid IS NULL
    BEGIN
        INSERT INTO dbo.[user] (UserSubname) 
        VALUES ( @sub);

        SET @userid = SCOPE_IDENTITY();
    END

    -- Optional: Keep the SELECT if you still want a result set returned
    SELECT @userid AS id;
END