namespace TheLibraryApi.Dtos;

public record BookItemDto(
    string Title,
    int Id,
    bool? IsCheckedOut,
    DateTime DueDate,
    int? UserId = null
    );

