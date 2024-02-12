using System;

namespace LibraryDatabase.Books.Queries.GetBooksById
{
    public class GetBooksByIdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public DateTime PublicationDate { get; set; }

        public string AuthorName { get; set; }
        public string AuthorNationality { get; set; }
        public DateTime AuthorBirthDate { get; set; }

    }
}
