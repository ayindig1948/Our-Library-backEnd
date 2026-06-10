using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTools.Models
{
  public class BookModel
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public required Author Author { get; set; }
        public int Id { get; set; }
        public int NumberOfCopies {  get; set; }
        
    }
}
