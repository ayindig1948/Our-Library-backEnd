using LibraryTools.Models;
using TheLibrayApi.Dtos;

namespace TheLibrayApi.EndPoints
{
    public static class Mapers
    {

        public static List<BookModelDto> MapToBookDto(List<BookModel> books)
        {
            List<BookModelDto> bookDtos = new List<BookModelDto>();
            foreach (var item in books)
            {
                bookDtos.Add(new BookModelDto(
                    Title: item.Title,
                    Description: item.Description,
                    Category: item.Category,
                    AuthorFirstName: item.Author.FirstName,
                    AuthorLastName: item.Author.LastName,
                    NumberOfCopies:item.NumberOfCopies
                    
                ));
            }

            return bookDtos;
        }
    }
}