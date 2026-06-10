using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using LibraryTools.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LibraryTools;

public class LibraryDataAsceses(string connectionStringName,[FromServices] IConfiguration configuration, ILogger<LibraryDataAsceses> logger) : ILibraryDataAsceses
{
    private readonly string _connectionString = configuration.GetConnectionString(connectionStringName)
          ?? throw new ArgumentException("Connection string not found.");
    private readonly ILogger _logger = logger;

    public async Task<int> AddUser(UserModel user)
    {
      
        var ids = await GetData<int, dynamic>("sP_GetUserId", new {  sub = user.SubId });
        return ids.FirstOrDefault();

    }
    public async Task<int> AddAuthor(Author author)
    {
        var ids = await GetData<int, dynamic>("sP_GetAutherId", new { firstName = author.FirstName, lastName = author.LastName });
        if (ids.FirstOrDefault() == 0 )
        {
            var newIds = await GetData<int, dynamic>("sP_AddAuther", new { firstName = author.FirstName, lastName = author.LastName });
            return newIds.FirstOrDefault();
        }
        return ids.FirstOrDefault();

    }
    public async Task AddBook(BookModel model,int numberOfCopies=1)
    {
        try
        {
            var aId = await GetAuthorId(model.Author);
            await SendData<dynamic>("sp_AddBook", new { title
                = model.Title, description = model.Description, category = model.Category, autherId = aId,numberOfCopies });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            throw new Exception("Could not Add a book model");
        }
    }
    public async Task AddBookItem(Author author, string title, int numberOfItems = 1)
    {
        try
        {
            var aId = await GetAuthorId(author);
            var bookId = await GetBookId(aId, title);
            for (int i = 0; i <= numberOfItems-1; i++)
            {
                await SendData<dynamic>("sp_AddBookItem", new { bookId, autherId = aId,  });
            }
           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            throw new Exception("Could not Add a book Item");
        }
    }
    public async Task<List<BookModel>> GetAllBooks()
    {
        try
        {
            using var coon = new SqlConnection(_connectionString);
            var data = await coon.QueryAsync<BookModel, Author, BookModel>("sp_get_AllBooks", (b, a) => { b.Author = a; return b; }, commandType: CommandType.StoredProcedure);

            return data.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            throw new Exception("could not connect");
        }

    }
    public async Task<List<BookModel>> GetAviBooks()
    {
        try
        {
            using var coon = new SqlConnection(_connectionString);
            var data = await coon.QueryAsync<BookModel, Author, BookModel>("sp_getAllAvlebelBooks", (b, a) => { b.Author = a; return b; }, commandType: CommandType.StoredProcedure);

            return data.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            throw new Exception("could not connect");
        }

    }
    public async Task<List<BookModel>> GetAviBooksByCategory(string category)
    {
        try
        {
            using var coon = new SqlConnection(_connectionString);
            var data = await coon.QueryAsync<BookModel, Author, BookModel>("sP_getAvlebelBooksByCategory", (b, a) => { b.Author = a; return b; }, new { category }, commandType: CommandType.StoredProcedure);

            return data.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            throw new Exception("could not connect");
        }

    }
    public async Task<int> GetNumberOfBooks(Author author)
    {
        var aId = await GetAuthorId(author);
        var numbers = await GetData<int, dynamic>("sP_Get_bookCount", new { authorId = aId });
        return numbers.FirstOrDefault();


    }
    public async Task<int?> CheckOutBook(Author author, string title,int userId  )
    {
        var CheckOutBooks = await GetOverdueBooks(userId);
        if (CheckOutBooks.Count > 3)
        {
            _logger.LogWarning("to Many Books Overdue for user {userId}", userId);
            return 0;
        }
        try
        {
            var aId = await GetAuthorId(author);
            var bookId = await GetBookId(aId, title);
            var id = await GetData<int?, dynamic>("sp_CheckOutBook", new { userId, bookId });
            return id.FirstOrDefault();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception("could not check out this time");


        }

    }
    public async Task CheckInBook(int userId, string title, Author author)
    {
        var trimTitle=NormalizeTitle(title);
        try
        {
            var aId = await GetAuthorId(author);
            var bookId = await GetBookId(aId,trimTitle);
       var numberList=     await GetData<int,dynamic>("sP_CheckInBook", new { userId, bookId });
            var number = numberList.FirstOrDefault();
            if (number > 0)
            {


                _logger.LogWarning("checked in a book:{id} title:{}", bookId, trimTitle);
            }
            else
            {
                _logger.LogWarning("could not find boock{t}", trimTitle);
                throw new Exception("could not find boock");

            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "could not check in  book {id}", trimTitle);
            throw new Exception("could not check in  book");
        }
    }
    public async Task<List<BookItem>> GetCheckedOutBooks(int userId)
    {
        var books = await GetData<BookItem, dynamic>("sP_get_AllCheckedOutBooks", new { userId });
        return books.ToList();
    }
    public async Task<List<BookItem>> GetOverdueBooks(int  userId)
    {
        var books = await GetData<BookItem, dynamic>("sP_get_OverDouBooks", new { userId });
        return books.ToList();
    }
    public async Task<List<BookModel>> SearchByCategory(string category)
    {
        try
        {
            using var coon = new SqlConnection(_connectionString);
            var books = await coon.QueryAsync<BookModel,Author,BookModel>("sP_get booksBy_Category", (b, a) => { b.Author = a; return b; }, new { category }, commandType: CommandType.StoredProcedure);
            return books.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception("could not search now");
        }
    }
    public async Task FulfilBook(int bookId)
    {
        await SendData<dynamic>("sP_FulfilBook", new { bookId });
    }
    public async Task<List<BookItem>> GetBooksToFulfil()
    {
        var books = await GetData<BookItem, dynamic>("sP_GetBooksToFulfil", new { });
        return books.ToList();
    }
    public  async Task RemoveBook(Author author, string title)
    {
        var trimTitle=NormalizeTitle(title);
        try
        {
            var aId = await GetAuthorId(author);
            var bookId = await GetBookId(aId, trimTitle);
            await SendData("sP_RemoveBookModel", new { id=bookId});
        }
        catch (Exception ex)
        {

          _logger.LogError($"Could not remove the book {ex.Message}");
            throw new Exception($"could not remove book{trimTitle}");

        }
        
        return;
    }
    public async Task<bool>RemoveBookItem(Author author ,string title)
    {
        bool otpout=false;
        var trimTitle = NormalizeTitle(title);
        try
        {
            var aId = await GetAuthorId(author);
            var bookId = await GetBookId(aId, trimTitle);
             otpout = await DeleteBookCopy(bookId);
        }
        catch (Exception ex)
        {


            _logger.LogError($"Could not remove the book item {ex.Message}");
           
       
        }

        return otpout;
    }
    private async Task<bool> DeleteBookCopy(int bookId)
    {
        using var conn = new SqlConnection(_connectionString);
        var deleted = await conn.ExecuteScalarAsync<int>(
            "sP_DeleteBookCopy",
            new { bookId },
            commandType: CommandType.StoredProcedure);

        return deleted == 1;
    }
    private async Task<int> GetBookId(int authorId, string title)

    {
        var trimTitle=NormalizeTitle(title);
        var listIds = await GetData<int, dynamic>("sP_GetBookId", new { authorId, titel = trimTitle });
        return listIds.FirstOrDefault();
    }
    private async Task<int> GetAuthorId(Author author)
    {
        var listId = await GetData<int, dynamic>("sP_GetAutherId", new { firstName = author.FirstName, lastName = author.LastName });
        return listId.FirstOrDefault();
    }

    private async Task<List<T>> GetData<T, U>(string StoredP, U par)
    {
        try
        {
            using var coon = new SqlConnection(_connectionString);
            var data = await coon.QueryAsync<T>(StoredP, par, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError("could not connect: {0} {proc}", ex.ToString() , StoredP);
            throw new Exception("cold not connect");
        }
    }
    private async Task SendData<T>(string StoredP, T par)
    {
        try
        {
            using var coon = new SqlConnection(_connectionString);
            await coon.ExecuteAsync(StoredP, par, commandType: CommandType.StoredProcedure);
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            throw new Exception("could not connect");
        }

    }
private static string NormalizeTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return "";

        // collapse any run of whitespace (spaces, tabs, newlines) to one space, then trim
        return Regex.Replace(title, @"\s+", " ").Trim();
    }


}

