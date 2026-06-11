using LibraryTools.Models;
using Microsoft.VisualBasic;
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
                    NumberOfCopies: item.NumberOfCopies

                ));
            }

            return bookDtos;
        }
        public static List<BookItemDto>MapToBookItemsDto(List<BookItem> books) {

            List<BookItemDto> bookDtos = new List<BookItemDto>();
            foreach (var item in books)
            {
                bookDtos.Add(new BookItemDto(
                   Title:item.Title,
                   IsCheckedOut:item.IsCheckedOut,
                   DueDate:item.DueDate,
    UserId :item.UserId,
    Id:item.Id


                ));
            }

            return bookDtos;
        }


    }
}

