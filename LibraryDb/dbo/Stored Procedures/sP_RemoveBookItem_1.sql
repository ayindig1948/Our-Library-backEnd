CREATE PROCEDURE [dbo].[sP_RemoveBookItem]
	@bookId int
AS
begin

        BEGIN TRANSACTION;
 
delete top(1) from [dbo].[Book] where BookId =@bookId and IsCheckedOut=0
IF @@ROWCOUNT = 1
begin
update dbo.BookModel set NumberOfCopies =NumberOfCopies-1 where BookModel.Id=@bookId
   COMMIT TRANSACTION;
            SELECT 1 AS Deleted;        
        END
        ELSE BEGIN
          
            ROLLBACK TRANSACTION;
            SELECT 0 AS Deleted;      
end
end;