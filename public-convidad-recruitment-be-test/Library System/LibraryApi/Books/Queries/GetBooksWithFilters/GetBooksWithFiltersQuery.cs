using MediatR;
using System;
using System.Collections.Generic;

namespace LibraryDatabase.Books.Queries.GetBooksWithFilters
{
    public class GetBooksWithFiltersQuery : IRequest<List<GetBooksWithFiltersDto>>
    {
        public GetBooksWithFiltersQuery(string title, string isbn, DateTime publicationDate )
        {
            Title = title;
            Isbn = isbn;
            PublicationDate = publicationDate;
        }

        public string Title { get; set; }
        public string Isbn { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
