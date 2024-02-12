using LibraryDatabase.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryDatabase.Books.Queries.GetBooksById
{
    public class GetBooksByIdHandler : IRequestHandler<GetBooksByIdQuery, GetBooksByIdDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetBooksByIdHandler> _logger;
        private readonly IStringLocalizer<Localizer> _localizer;
        private readonly IValidationFailureService _validation;

        public GetBooksByIdHandler(
            IApplicationDbContext context,
            ILogger<GetBooksByIdHandler> logger,
            IStringLocalizer<Localizer> localizer,
            IValidationFailureService validation)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public async Task<GetBooksByIdDto> Handle(GetBooksByIdQuery request, CancellationToken cancellationToken)
        {

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                
                var book = await _context
                    .Books
                    .Where(b=> b.Id == request.Id)
                    .Select(b=> new GetBooksByIdDto {
                        Title = b.Title,
                        PublicationDate = b.PublicationDate,
                        Isbn = b.Isbn
                    })
                    .AsNoTracking()
                    .SingleOrDefaultAsync(cancellationToken);
                if (book is null)
                {
                    throw new NotFoundException(nameof(book), nameof(book.Id));
                }

                if (request.Author == true)
                {
                    var author = await _context
                        .Authors
                        .Include(a => a.Book)
                        .SingleOrDefaultAsync(a => a.Book.Id == request.Id, cancellationToken);

                    if (author != null)
                    {
                        book.AuthorBirthDate = author.BirthDate;
                        book.AuthorName = author.Name;
                        book.AuthorNationality = author.Nationality;
                    }

                }
                return book;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting books by id: {@Error}", ex);
                await transaction.DisposeAsync();
                throw;
            }
            throw new NotImplementedException();
        }
    }
}
