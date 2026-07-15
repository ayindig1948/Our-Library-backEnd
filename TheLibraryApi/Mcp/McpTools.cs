using System.ComponentModel;
using System.Security.Claims;
using System.Security.Principal;
using LibraryTools;
using LibraryTools.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using TheLibraryApi.Dtos;
using TheLibraryApi.EndPoints;
using static Auth0.AspNetCore.Authentication.Api.Auth0Constants.CustomDomains.Error;

namespace TheLibraryApi.Mcp;

[McpServerToolType]
public class McpTools
{
    private ILibraryDataAcsees _dataAcsees;
    private ILogger<McpTools> _logger;
    private IOutputCacheStore _outputCacheStore;
    private ClaimsPrincipal _principal;

    // Update the constructor to accept IOutputCacheStore
    public McpTools(ILibraryDataAcsees dataAcsees, ILogger<McpTools> log, IOutputCacheStore outputCacheStore,ClaimsPrincipal principal)
    {
        _dataAcsees = dataAcsees;
        _logger = log;
        _outputCacheStore = outputCacheStore;
        _principal= principal;
    }


    [McpServerTool, Description(" a method to add a book and check if the author is there if not it adds it")]
    public async Task<string> AddBook(AddBookRequest request)
    {
        _logger.LogInformation("NumberOfItems = {N}", request.NumberOfItems);

        var author = new Author
        {
            FirstName = request.AuthorFirstName,
            LastName = request.AuthorLastName
        };
        try
        {
            await _dataAcsees.AddAuthor(author
            );
            var book = new BookModel
            {
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                Author = author,


            };
            await _dataAcsees.AddBook(book, 0)
            ;
            _logger.LogInformation("Book added: {Title} by {Author} by llm", book.Title, $"{author.FirstName} {author.LastName}");
            await _dataAcsees.AddBookItem(author, request.Title, (int)request.NumberOfItems);
            await _outputCacheStore.EvictByTagAsync("CacheAll", CancellationToken.None);
            return $"Book '{book.Title}' by {author.FirstName} {author.LastName} added successfully with {request.NumberOfItems} copies.";
        }
        catch (Exception ex)
        {
            _logger.LogError("Error adding book: {exceptionMessage} by llm", ex.Message);
            return $"Error adding book: {request.Title}";
        }
    }
    [McpServerTool, Description(" a method to add a book copy of an existing book")]
    public  async Task<string> AddBookItem(string authorFirstName, string authorLastName, string title, int numberOfItems)
    {
            try { 
        var author = new Author
        {
            FirstName = authorFirstName,
            LastName = authorLastName
        };
        await _dataAcsees.AddBookItem(author, title, numberOfItems);
        await _outputCacheStore.EvictByTagAsync("CacheAll", CancellationToken.None);
        return $"Added {numberOfItems} copies of '{title}' by {authorFirstName} {authorLastName}.";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding book item: {exceptionMessage} by llm", ex.Message);
                return $"Error adding book item: {ex.Message}";
            }
        }
    [McpServerTool, Description(" a method to check out a book copy")]
    public  async Task<string> CheckOutBook(CheckoutRequest request )
  


    {
        try
        {
            var author = new Author
            {

                FirstName = request.AuthorFirstName,
                LastName = request.AuthorLastName
            };

            var userId = await DataEndPoints .GetUserId(_principal, _dataAcsees);   // pass the real principal
            var result = await _dataAcsees.CheckOutBook(author, request.Title, userId);
            if
            (result is null)
            {
                _logger.LogWarning("That book {book} isn't available to check out.", request.Title);
                return $"That book '{request.Title}' isn't available to check out.";

            }
            if (result == 0)
            {
                return $"User has too many overdue books.";
            }

            await _outputCacheStore.EvictByTagAsync("CacheAll", CancellationToken.None);
            return $"Book '{request.Title}' checked out successfully.";
        }
        catch (Exception ex)
        {
           _logger.LogError("Error checking out book: {exceptionMessage} by llm", ex.Message);
         
        }
    }
    [McpServerTool, Description(" a method to check in a book copy")]
    public  async Task<string> CheckInBook( CheckInRequest request)
    {
        int userId;
        try
        {
            userId = await DataEndPoints. GetUserId(_principal, _dataAcsees);

        }
        catch (Exception ex)
        {
            _logger.LogWarning("Unable to determine user ID from claims. Exception: {exceptionMessage}", ex.Message);
            return $"Unable to determine user ID from claims. "; 
        }
        var author = new Author
        {
            FirstName = request.AuthorFirstName,
            LastName = request.AuthorLastName
        };
        try
        {

            await _dataAcsees.CheckInBook(userId, request.Title, author);
            _logger.LogInformation("User ID {userId} checked in book {bookTitle} by {authorFirstName} {authorLastName} by the llm.", userId, request.Title, author.FirstName, author.LastName);
            await _outputCacheStore.EvictByTagAsync("CacheAll", CancellationToken.None);
            return $"Book '{request.Title}' checked in successfully."; 
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Error during check-in for user ID {userId} and book {bookTitle}. Exception: {exceptionMessage} by the llm.", userId, request.Title, ex.Message);
            return $"Error checking in book: {request.Title}";
        }
    }
    [McpServerTool, Description(" a method to get the list of books that need to be fulfilled")]
    public async  Task<string> GetBooksToFulfil()
    {
        try
        {

            var books = await _dataAcsees.GetBooksToFulfil();


            var dtoList = Mappers.MapToBookItemsDto(books);

            _logger.LogWarning("getting fulfilled books list by the llm ");
            return $"Books to fulfill: {string.Join(", ", dtoList.Select(b => b.Title))}"   
        }
        catch (Exception ex)
        {
            _logger.LogError("could not get list to fulfill by the llm: {exceptionMessage}", ex.Message);
            return $"Error retrieving books to fulfill: {ex.Message}";
        }

    }
    [McpServerTool,Description(" a method to fulfill a book copy")]
    public  async Task<string> FulfilBook( int BookId )
    {
        try
        {
            await _dataAcsees.FulfilBook(BookId);
            await _outputCacheStore.EvictByTagAsync("CacheAll", CancellationToken.None);
            return $"Book with ID {BookId} has been fulfilled.";
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fulfilling book with ID {BookId}: {exceptionMessage} by the llm", BookId, ex.Message);
            return $"Error fulfilling book with ID {BookId}: {ex.Message}";
        }
    }
    [McpServerTool, Description(" a method to remove a book and all its copies")]
    public async Task<string> RemoveBook(CheckInRequest request)
    {
        var author = new Author
        {
            FirstName = request.AuthorFirstName,
            LastName = request.AuthorLastName
        };
        try
        {


            await _dataAcsees.RemoveBook(author, request.Title);
            _logger.LogInformation("Book removed: {Title} by {Author} by the llm", request.Title, $"{author.FirstName} {author.LastName}");

            await _outputCacheStore.EvictByTagAsync("CacheAll", CancellationToken.None);
            return $"Book removed: {request.Title} by {author.FirstName} {author.LastName}";
        } catch (Exception ex) {
            _logger.LogError("Error removing book: {exceptionMessage} by the llm", ex.Message);
            return $"Error removing book: {request.Title} by {author.FirstName} {author.LastName}";
        }
    }
    [McpServerTool, Description(" a method to edit a book's details")]
    public async Task<string> EditBooks(EditRequst requst)
    {
        try
        {

            await _dataAcsees.EditBook(requst.BookId, requst.Title, requst.Description, requst.Category);
            await _outputCacheStore.EvictByTagAsync("CacheAll", CancellationToken.None);
            _logger.LogInformation("Book edited: {Title} by the llm", requst.Title);
            return $"Book edited: {requst.Title}";
        }
        catch (Exception ex)
        {
            _logger.LogError("Error editing book: {exceptionMessage} by the llm", ex.Message);
            return $"Error editing book: {requst.Title}";

        }
    }
    [McpServerTool, Description(" a method to get all books in the library")]
    public  async Task<string> GetAllBooks()
    {
        var books = await _dataAcsees.GetAllBooks();
        List<BookModelDto> bookDtos = Mappers.MapToBookDto(books);
        _logger.LogInformation("Retrieved all books by the llm");
        return $"All books: {string.Join(", ", bookDtos.Select(b => b.Title))}";
    }

    [McpServerTool, Description(" a method to remove a book copy")]
    public  async Task<string> RemoveBookItem([Description("book title and author name")]CheckInRequest
        request)
    {
        var author = new Author
        {
            FirstName = request.AuthorFirstName,
            LastName = request.AuthorLastName
        };
        try
        {

       
                var output = await _dataAcsees.RemoveBookItem(author, request.Title);
                if (output == true)
                {
            await _outputCacheStore .EvictByTagAsync("CacheAll", CancellationToken.None);
            _logger.LogInformation("Book item removed: {Title} by {Author} by the llm", request.Title, $"{author.FirstName} {author.LastName}");
            return $"Book item removed: {request.Title} by {author.FirstName} {author.LastName}";

        }
                
        return $"Item not found: {request.Title}";

    }
        catch (Exception ex)
        {
            _logger.LogError("Error removing book item: {exceptionMessage} by the llm", ex.Message);
            return $"Error removing book item: {request.Title} by {author.FirstName} {author.LastName}";
        }
    }
    public async Task<int> GetNumberOfBooks(string authorFirstName, string authorLastName)
{
    var author = new Author
    {
        FirstName = authorFirstName,
        LastName = authorLastName
    };
    var numberOfBooks = await _dataAcsees.GetNumberOfBooks(author);
    return numberOfBooks;
}
    public async Task<List<string>> GetAviBooksByCategory(string category)
    {
        try { 
            var books = await _dataAcsees.GetAviBooksByCategory(category);
        List<BookModelDto> bookDtos = Mappers.MapToBookDto(books);

        _logger.LogInformation("Retrieved available books in category: {Category} by the llm", category);
        return bookDtos.Select(b => b.Title).ToList();
        }   
        catch
        {
            _logger.LogWarning("No available books found in category: {Category} by the llm", category);
            return $"No books found in category: {category}".Split(',').ToList();
        }
    }
    public async Task<List<string>> SearchByCategory(string category)
    {
        try {

            var books = await _dataAcsees.SearchByCategory(category);
            List<BookModelDto> bookDtos = Mappers.MapToBookDto(books);
            _logger.LogInformation("Retrieved books in category: {Category} by the llm", category);

            return bookDtos.Select(b => b.Title).ToList();
        }
        catch
        {
            _logger.LogWarning("No books found in category: {Category} by the llm", category);
            return $"No books found in category: {category}".Split(',').ToList();
        }

    }
}
