using System.Security.Claims;
using LibraryTools;
using LibraryTools.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using TheLibraryApi.Dtos;
using static Auth0.AspNetCore.Authentication.Api.Auth0Constants.CustomDomains.Error;

namespace TheLibraryApi.EndPoints
{
  
    public static class DataEndPoints 
    {
        public static async Task<IResult> AddBook([FromBody] AddBookRequest request, [FromServices] ILibraryDataAcsees libraryData, IOutputCacheStore cache, ILogger<Program> logger)
        {
            logger.LogInformation("NumberOfItems = {N}", request.NumberOfItems);

            var author = new Author
            {
                FirstName = request.AuthorFirstName,
                LastName = request.AuthorLastName
            };
            await libraryData.AddAuthor(author
            );
            var book = new BookModel
            {
                Title = request.Title,
                Description = request.Description,
                Category = request.Category,
                Author = author,
                
                
            };
            await libraryData.AddBook(book,0)
            ;
            await libraryData.AddBookItem(author, request.Title, (int)request.NumberOfItems); 
            await cache.EvictByTagAsync("CacheAll", CancellationToken.None);
            return Results.Ok();
        }
        public static async Task<IResult> AddBookItem([FromBody] AddBookItemRequest request, [FromServices] ILibraryDataAcsees libraryData, [FromServices] IOutputCacheStore cache)
        {
            var author = new Author
            {
                FirstName = request.AuthorFirstName,
                LastName = request.AuthorLastName
            };
            await libraryData.AddBookItem( author, request.Title, (int)request.NumberOfItems);
            await cache.EvictByTagAsync("CacheAll", CancellationToken.None);
            return Results.Ok();
        }
        public static async Task<IResult> EditBooks([FromBody]EditRequst requst, [FromServices]ILibraryDataAcsees libraryData,ILogger<Program> logger, IOutputCacheStore cache)
        {
            await libraryData.EditBook(requst.BookId, requst.Title, requst.Description ,requst.Category);
            await cache.EvictByTagAsync("CacheAll", CancellationToken.None);

            return Results.Ok();
          
        }
        public static async Task<IResult> GetAllBooks([FromServices] ILibraryDataAcsees libraryData)
        {
            var books = await libraryData.GetAllBooks();
            List<BookModelDto> bookDtos = Mappers.MapToBookDto(books);
            return Results.Ok(bookDtos);
        }

        public static async Task<IResult> GetAviBooks([FromServices] ILibraryDataAcsees libraryData)
        {
            var books = await libraryData.GetAviBooks();
            List<BookModelDto> bookDtos = Mappers.MapToBookDto(books);
            
            return Results.Ok(bookDtos);
        }
        public static async Task<IResult> GetAviBooksByCategory(string category, [FromServices] ILibraryDataAcsees libraryData)
        {
            var books = await libraryData.GetAviBooksByCategory(category);
            List<BookModelDto> bookDtos = Mappers.MapToBookDto(books);
            return Results.Ok(bookDtos);
        }
        public static async Task<IResult> SearchByCategory([FromRoute] string category, [FromServices] ILibraryDataAcsees libraryData)
        {
            var books = await libraryData.SearchByCategory(category);
            List<BookModelDto> bookDtos = Mappers.MapToBookDto(books);
            
            return Results.Ok(bookDtos);
        }
        public static async Task<IResult> GetNumberOfBooks([FromRoute] string authorFirstName, [FromRoute] string authorLastName, [FromServices] ILibraryDataAcsees libraryData)
        {
            var author = new Author
            {
                FirstName = authorFirstName,
                LastName = authorLastName
            };
            var numberOfBooks = await libraryData.GetNumberOfBooks(author);
            return Results.Ok(numberOfBooks);
        }
        public static async Task<IResult> GetOverdueBooks(int userId, [FromServices] ILibraryDataAcsees libraryData)
        {
            var books = await libraryData.GetOverdueBooks(userId);
            var bookDtos = Mappers.MapToBookItemsDto(books);
            
            return Results.Ok(bookDtos);
        }
        private static async Task<int> GetUserId(ClaimsPrincipal claim, ILibraryDataAcsees libraryData)
        {
            var user11 = new UserModel
            {

                


                SubId =claim.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "Unknown"
            };
          var id=await  libraryData.AddUser(user11);
          return id;
        }

        public static async Task<IResult> CheckOutBook(
    [FromBody] CheckoutRequest request,
    ClaimsPrincipal user,                         // <-- injected by the framework
    [FromServices] ILibraryDataAcsees libraryData,
    [FromServices] ILogger<Program> logger,
    IOutputCacheStore cache)
        {
            try
            {
                var author = new Author
                {
                    
                    FirstName = request.AuthorFirstName,
                    LastName = request.AuthorLastName
                };

                var userId = await GetUserId(user, libraryData);   // pass the real principal
                var result = await libraryData.CheckOutBook(author, request.Title, userId);
                if 
                (result is null) {
                    logger.LogWarning("That book {book} isn't available to check out.", request.Title);
    return Results.Conflict("That book isn't available to check out.");
                    
                  }
                await cache.EvictByTagAsync("CacheAll", CancellationToken.None);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        public static async Task<IResult> CheckInBook([FromBody] CheckInRequest request, ClaimsPrincipal user, ILogger<Program> logger, [FromServices] ILibraryDataAcsees libraryData, IOutputCacheStore cache)
        {
            int userId;
            try
            {
                userId = await GetUserId(user, libraryData);

            }
            catch (Exception ex)
            {
                logger.LogWarning("Unable to determine user ID from claims. Exception: {exceptionMessage}", ex.Message);
                return Results.BadRequest("Unable to determine user ID. Please ensure you are authenticated.");
            }
            var author = new Author
            {
                FirstName = request.AuthorFirstName,
                LastName = request.AuthorLastName
            };
            try
            {

                await libraryData.CheckInBook(userId, request.Title, author);
                logger.LogInformation("User ID {userId} checked in book {bookTitle} by {authorFirstName} {authorLastName}.", userId, request.Title, author.FirstName, author.LastName);
                await cache.EvictByTagAsync("CacheAll", CancellationToken.None);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                logger.LogWarning("Error during check-in for user ID {userId} and book {bookTitle}. Exception: {exceptionMessage}", userId, request.Title, ex.Message);
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }
        public static async Task<IResult> GetCheckedOutBooks(ClaimsPrincipal user, ILogger<Program> logger, ILibraryDataAcsees libraryData)
        {
            int userId;
            try
            {
                userId = await GetUserId(user, libraryData);
            }
            catch (Exception ex)
            {
                logger.LogWarning("Unable to determine user ID from claims. Exception: {exceptionMessage}", ex.Message);

                return Results.BadRequest("Unable to determine user ID. Please ensure you are authenticated.");
            }
            try
            {
                var books = await libraryData.GetCheckedOutBooks(userId);
                var bookDtos = Mappers.MapToBookItemsDto(books);
                
                return Results.Ok(bookDtos);

            }
            catch (Exception ex)
            {
                logger.LogWarning("Error retrieving checked out books for user ID {userId}. Exception: {exceptionMessage}", userId, ex.Message);
                return Results.Problem(detail: ex.Message, statusCode: 500);

            }
        }
        public static async Task<IResult> GetBooksToFulfil(ILibraryDataAcsees libraryData,ILogger<Program> logger)
        {
            try
            {

                var books = await libraryData.GetBooksToFulfil();


                var dtoList = Mappers.MapToBookItemsDto(books);
                
                logger.LogWarning("getting fulfilled books list ");
                return Results.Ok(dtoList);
            }
            catch (Exception ex) {
                logger.LogError("could not get list to fulfill");
                return Results.Problem();
            }
           
        }
        public static async Task<IResult>FulfilBook(ILibraryDataAcsees libraryData,int BookId,IOutputCacheStore cache) 
        {
            await libraryData.FulfilBook(BookId);
            await cache.EvictByTagAsync("CacheAll", CancellationToken.None);
            return Results.Ok();    
        }
        public static async Task<IResult> RemoveBook([FromBody]CheckInRequest request,[FromServices] ILibraryDataAcsees libraryData, IOutputCacheStore cache)
        {
           var author = new Author
           {
               FirstName = request.AuthorFirstName,
               LastName = request.AuthorLastName
           };
            await libraryData.RemoveBook(author,request.Title);

            await cache.EvictByTagAsync("CacheAll", CancellationToken.None);
            return Results.Ok();
        }
        public static async Task<IResult> RemoveBookItem([FromBody]CheckInRequest request,[FromServices]  ILibraryDataAcsees libraryData,IOutputCacheStore cache)
        {
            var author = new Author
            {
                FirstName = request.AuthorFirstName,
                LastName = request.AuthorLastName
            };
           var output= await libraryData.RemoveBookItem(author, request.Title);
            if (output==true)
            {
                await cache.EvictByTagAsync("CacheAll", CancellationToken.None);
                return Results.Ok();

            }
            return Results.NotFound("item not found");
           
        }

        //// the code for add user info for production should be more secure and should not allow anyone to add user but for now we will just add this endpoint for testing purpose
        //public static async Task<IResult> AddUser([FromBody] AddUserRequst request, [FromServices] ILibraryDataAcsees libraryData)
        //{
        //    var user = new UserModel
        //    {
        //        FirstName = request.FirstName,
        //        LastName = request.LastName

        //    };
        //    var id = await libraryData.AddUser(user);
        //    return Results.Ok(id);
        //}
        //public record AddUserRequst(
        //    string FirstName,
        //    string LastName,
        //    DateTime DateOfBirth
        //    );
        public static WebApplication MapDataEndPoints(this WebApplication app)
        {

            app.MapGet("/BooksToFulfil", GetBooksToFulfil).CacheOutput("AdminCache").RequireAuthorization("write:books");

            app.MapGet("/getallbooks", GetAllBooks).CacheOutput("CacheAll").RequireRateLimiting("FixedPolicy").RequireAuthorization("read:books");
            app.MapGet("/getaviLebelbooks", GetAviBooks).CacheOutput("CacheAll").RequireRateLimiting("FixedPolicy").RequireAuthorization("read:books");
            app.MapGet("/getaviLebelbooksbycategory/{category}", GetAviBooksByCategory).CacheOutput("CacheAll").RequireRateLimiting("FixedPolicy").RequireAuthorization("read:books");
            app.MapGet("/searchbycategory/{category}", SearchByCategory).CacheOutput("CacheAll").RequireRateLimiting("FixedPolicy").RequireAuthorization("read:books");
            app.MapGet("/getnumberofbooks/{authorFirstName}/{authorLastName}", GetNumberOfBooks).CacheOutput("CacheAll").RequireRateLimiting("FixedPolicy").RequireAuthorization("read:books");
            app.MapGet("/getoverduebooks/{userId}", GetOverdueBooks).CacheOutput("AdminCache").RequireAuthorization("read:books");
            app.MapGet("/getcheckedoutbooks", GetCheckedOutBooks).CacheOutput("CacheAll").RequireAuthorization("read:books").RequireRateLimiting("FixedPolicy");
            app.MapPost("/addbookitem", AddBookItem).RequireAuthorization("write:books");
            app.MapPost("/addbook", AddBook).RequireAuthorization("write:books");
            app.MapPut("/checkoutbook", CheckOutBook).RequireAuthorization("read:books");
            app.MapPut("/editbook", EditBooks);
            app.MapPut("/FulfilBook/{BookId}", FulfilBook).RequireAuthorization("write:books");
            app.MapPut("/checkinbook", CheckInBook).RequireAuthorization("read:books");
            app.MapDelete("/removebook", RemoveBook).RequireAuthorization("write:books");
            app.MapDelete("/removebookitem", RemoveBookItem).RequireAuthorization("write:books");
           // app.MapPost("/adduser", AddUser);
            return app;
        }
    }
}
