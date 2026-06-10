using LibraryTools.Models;

namespace TheLibrayApi.Dtos;

public record BookModelDto(
    string Title,
 string Description,
 string Category,
   string AuthorFirstName,
    string AuthorLastName,
    int NumberOfCopies
 );