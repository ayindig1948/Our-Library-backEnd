namespace TheLibraryApi.Dtos
{
    public record CheckoutRequest(
     string Title,
     string AuthorFirstName,
     string AuthorLastName);
}
