using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTools.Models;
public  class UserModel
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string SubId { get; set; }
    //  public DateOnly DateOfBirth { get; set; } =new DateOnly();
    public List<BookItem>? BookItems { get; set; }=new List<BookItem>();
}
