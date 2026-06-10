using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTools.Models
{
  public  class BookItem
    { public int Id { get; set; }
       
 
   public int   UserId {  get; set; }    
    public bool IsCheckedOut {  get; set; }
    public DateTime DueDate { get; set; } = new DateTime();
        
    public required string Title { get; set; }
    }
}
