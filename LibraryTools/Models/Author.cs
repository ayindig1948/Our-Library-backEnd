using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTools.Models
{
    public class Author
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
