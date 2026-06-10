namespace TheLibrayApi.Dtos
{
   public record AddBookItemRequest(
    string Title,
    string AuthorFirstName,
    string AuthorLastName,
       int? NumberOfItems = null
    );
}