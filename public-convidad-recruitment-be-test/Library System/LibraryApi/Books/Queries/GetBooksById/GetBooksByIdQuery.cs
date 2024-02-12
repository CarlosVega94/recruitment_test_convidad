using MediatR;

namespace LibraryDatabase.Books.Queries.GetBooksById
{
    public class GetBooksByIdQuery : IRequest<GetBooksByIdDto> 
    {
        public GetBooksByIdQuery(int id, bool author) 
        { 
            Id = id;
            Author = author;
        }

        public int Id { get; set; }
        public bool Author { get; set; }
    }
}
