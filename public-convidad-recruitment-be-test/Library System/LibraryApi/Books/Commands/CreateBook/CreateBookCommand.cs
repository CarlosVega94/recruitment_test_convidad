using MediatR;
using System;
using System.Collections.Generic;

namespace LibraryDatabase.Books.Commands.CreateBook

{
    public class CreateBookCommand : IRequest<int>
    {
        public CreateBookCommand(
            string title,
            string isbn,
            DateTime publicationDate)
        {
            Title = title;
            ISBN = isbn;
            PublicationDate = publicationDate;
        }
        public string Title { get; }
        public string ISBN { get; }
        public DateTime PublicationDate { get; }
        public int AuthorId { get; }
    }

}
