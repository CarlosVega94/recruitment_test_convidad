using LibraryDatabase.Books.Queries.GetBooksWithFilters;
using LibraryDatabase.Entitites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryDatabase.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryDatabase.Books.Queries.GetBooksById;
using LibraryDatabase.Books.Commands.CreateBook;

namespace LibraryDatabase.Controllers
{
    [Route("api/[controller]")]
    public class BookController : ApiControllerBase
    {
        public Dictionary<int, Book> Librarybooks = new Dictionary<int, Book>();
        public Dictionary<int, Author> Libraryauthors = new Dictionary<int, Author>();

        public BookController(Dictionary<int, Book> librarybooks)
        {
            this.Librarybooks = librarybooks;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("title/{title}/isbn/{isbn}/publicationdate/{publicationDate}")]
        public async Task<ActionResult<List<GetBooksWithFiltersDto>>> GetAllBooks(string title, string isbn, DateTime publicationDate)
        {
            return await Mediator.Send(new GetBooksWithFiltersQuery(title, isbn, publicationDate));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}/author/{author}")]
        public async Task<ActionResult<GetBooksByIdDto>> getbook(int id, bool author)
        {
            return await Mediator.Send(new GetBooksByIdQuery(id, author));
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<int>> AddBook(CreateBookCommand command)
        {
            var id = await Mediator.Send(command);
            var author = true;
            return CreatedAtAction(nameof(getbook), new { id , author }, id);
        }
    }
}