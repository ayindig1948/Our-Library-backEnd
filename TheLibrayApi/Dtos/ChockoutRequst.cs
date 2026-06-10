namespace TheLibrayApi.Dtos
{
    public record CheckoutRequest(
     string Title,
     string AuthorFirstName,
     string AuthorLastName);
}
