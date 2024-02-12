using LibraryDatabase.Books.Queries.GetBooksWithFilters;
using LibraryDatabase.Entitites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryDatabase.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryDatabase.Authors.Commands.CreateAuthor;

namespace LibraryDatabase.Controllers
{
    [Route("api/[controller]")]
    public class AuthorController : ApiControllerBase
    {
        public Dictionary<int, Book> Librarybooks = new Dictionary<int, Book>();
        public Dictionary<int, Author> Libraryauthors = new Dictionary<int, Author>();

        public AuthorController(Dictionary<int, Author> libraryauthors)
        {
            this.Libraryauthors = libraryauthors;
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<int>> AddAuthor(CreateAuthorCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}