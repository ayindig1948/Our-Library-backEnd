-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[sp_CheckOutBook] 
    @bookId int, 
    @UserId int
AS 
BEGIN 
    SET NOCOUNT ON; 
    DECLARE @id int; -- Changed from parameter to local variable

    BEGIN TRANSACTION;

    -- 1. Try to find and lock an available copy
    SELECT TOP 1 @id = b.Id 
    FROM dbo.Book b WITH (ROWLOCK, UPDLOCK, READPAST) 
    WHERE b.BookId = @bookId AND b.IsCheckedOut = 0;

    IF @id IS NOT NULL 
    BEGIN
        -- 2. Success: Update the record and COMMIT
        UPDATE dbo.Book 
        SET UserId = @UserId, 
            IsCheckedOut = 1, 
            DueDate = DATEADD(WEEK, 2, GETDATE())
        WHERE Id = @id;
		update dbo.BookModel  set NumberOfCopies=NumberOfCopies-1 where dbo.BookModel.Id=@bookId

        COMMIT TRANSACTION;
    END
    ELSE 
    BEGIN
        -- 3. Failure: ROLLBACK and ensure @id is null
		 SET @id = null
        ROLLBACK TRANSACTION;
      
    END

    -- 4. Return the ID back to your application (C#, etc.)
    SELECT @id AS CheckedOutId;
END