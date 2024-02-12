using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LibraryDatabase.Authors.Commands.CreateAuthor;
using LibraryDatabase.Entitites;
using LibraryDatabase.Common;

namespace LibraryDatabase.Books.Commands.CreateBook
{
    public class CreateBookHandler : IRequestHandler<CreateBookCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateBookHandler> _logger;
        private readonly IStringLocalizer<Localizer> _localizer;
        private readonly IValidationFailureService _validation;

        public CreateBookHandler(
            IApplicationDbContext context,
            ILogger<CreateBookHandler> logger,
            IStringLocalizer<Localizer> localizer,
            IValidationFailureService validation)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book();
            book.Title = request.Title;
            book.Isbn = request.ISBN;
            book.PublicationDate = request.PublicationDate;

            await ValidationAsync(book, cancellationToken);

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var booksAuthor = await _context
                    .Authors
                    .SingleAsync(a => a.Id == request.AuthorId, cancellationToken);

                booksAuthor.Books.Add(book);

                await _context.Books.AddAsync(book, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.Authors.AddAsync(booksAuthor, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return book.Id;
            }
            catch (Exception e)
            {
                _logger.LogCritical("Error creating a book: {@Error}", e);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task ValidationAsync(Book entity, CancellationToken cancellationToken)
        {
            var book = await _context
                    .Books
                    .SingleOrDefaultAsync(
                        a => a.Title == entity.Title && a.Isbn == entity.Isbn && a.PublicationDate == entity.PublicationDate, cancellationToken);

            if (book != null)
            {
                _validation.Add(
                nameof(entity.Title),
                _localizer["Ya existe este libro"]);
            }

            _validation.AddAndRaiseException(
                nameof(book.Title),
                _localizer["Ya existe un libro con este mismo titulo"]);
        }
    }
}
