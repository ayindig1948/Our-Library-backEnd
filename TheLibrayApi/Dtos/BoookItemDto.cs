namespace TheLibrayApi.Dtos;

public record BoookItemDto(
    string Title,
    int Id,
    bool? IsCheckedOut,
    DateTime DueDate,
    int? UserId = null
    );

