using System;
using System.Collections.Generic;

namespace LibraryDatabase.Entitites
{
    public class Author
    {
        public Author() 
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<Book> Books { get; set; }
        public Book Book { get; set; }
    }
}