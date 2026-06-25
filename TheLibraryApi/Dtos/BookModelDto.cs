using LibraryTools.Models;

namespace TheLibraryApi.Dtos;

public record BookModelDto(
    int Id,
    string Title,
 string Description,
 string Category,
   string AuthorFirstName,
    string AuthorLastName,
    int NumberOfCopies
 );