using LibraryDatabase.Books.Queries.GetBooksById;
using LibraryDatabase.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryDatabase.Books.Queries.GetBooksWithFilters
{
    public class GetBooksWithFiltersHandler : IRequestHandler<GetBooksWithFiltersQuery, List<GetBooksWithFiltersDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetBooksWithFiltersHandler> _logger;
        private readonly IStringLocalizer<Localizer> _localizer;
        private readonly IValidationFailureService _validation;

        public GetBooksWithFiltersHandler(
            IApplicationDbContext context,
            ILogger<GetBooksWithFiltersHandler> logger,
            IStringLocalizer<Localizer> localizer,
            IValidationFailureService validation)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public async Task<List<GetBooksWithFiltersDto>> Handle(GetBooksWithFiltersQuery request, CancellationToken cancellationToken)
        {

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var books = await _context
                    .Books
                    .Where(p=>p.Id > 0 && (
                        p.Title.Contains(request.Title ?? "")
                        ||
                        p.Isbn.Contains(request.Isbn ?? "")
                        ||
                        p.PublicationDate >= request.PublicationDate
                        ))
                    .Select(p => new GetBooksWithFiltersDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Isbn = p.Isbn,
                        PublicationDate = p.PublicationDate
                    })
                .ToListAsync(cancellationToken);
                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting books by filter: {@Error}", ex);
                await transaction.DisposeAsync();
                throw;
            }
        }
    }
}
