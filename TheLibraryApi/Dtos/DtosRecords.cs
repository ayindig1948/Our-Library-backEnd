using System.ComponentModel.DataAnnotations;

namespace TheLibraryApi.Dtos;
public record AddBookRequest(
    [MaxLength(50, ErrorMessage = "Title cannot be longer than 50 characters")]
    
string Title,
    [MaxLength(250, ErrorMessage = "Description cannot be longer than 250 characters")]
    string Description,
    [MaxLength(50, ErrorMessage = "Category cannot be longer than 50 characters")]
    string Category,
    [MaxLength(50, ErrorMessage = "Author first name cannot be longer than 50 characters")]
    string AuthorFirstName,
    [MaxLength(50, ErrorMessage = "Author last name cannot be longer than 50 characters")]
    string AuthorLastName,
    int? NumberOfItems = null
    );
   
