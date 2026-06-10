using LibraryTools.Models;

namespace LibraryTools
{
    public interface ILibraryDataAsceses
    {
        Task<int> AddAuthor(Author author);
        Task AddBook(BookModel model,int numberOfCopie);
        Task AddBookItem(Author author, string title, int numberOfItems);
        Task<int> AddUser(UserModel user);
        Task CheckInBook(int userId, string title, Author author);
        Task<int?> CheckOutBook(Author author, string title, int userId);
        Task FulfilBook(int bookId);
        Task<List<BookModel>> GetAllBooks();
        Task<List<BookModel>> GetAviBooks();
        Task<List<BookModel>> GetAviBooksByCategory(string category);
        Task<List<BookItem>> GetBooksToFulfil();
        Task<List<BookItem>> GetCheckedOutBooks(int userId);
        Task<int> GetNumberOfBooks(Author author);
        Task<List<BookItem>> GetOverdueBooks(int userId);
        Task RemoveBook(Author author, string title);
        Task<bool> RemoveBookItem(Author author, string title);
        Task<List<BookModel>> SearchByCategory(string category);
    }
}