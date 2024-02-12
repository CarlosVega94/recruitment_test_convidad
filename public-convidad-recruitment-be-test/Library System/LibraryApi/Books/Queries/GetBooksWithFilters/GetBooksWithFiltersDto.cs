using System;

namespace LibraryDatabase.Books.Queries.GetBooksWithFilters
{
    public class GetBooksWithFiltersDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
