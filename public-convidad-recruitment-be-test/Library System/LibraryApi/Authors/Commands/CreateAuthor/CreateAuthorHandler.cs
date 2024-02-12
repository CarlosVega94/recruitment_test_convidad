using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LibraryDatabase.Entitites;
using LibraryDatabase.Common;

namespace LibraryDatabase.Authors.Commands.CreateAuthor
{
    public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateAuthorHandler> _logger;
        private readonly IStringLocalizer<Localizer> _localizer;
        private readonly IValidationFailureService _validation;

        public CreateAuthorHandler(
            IApplicationDbContext context,
            ILogger<CreateAuthorHandler> logger,
            IStringLocalizer<Localizer> localizer,
            IValidationFailureService validation)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _validation = validation ?? throw new ArgumentNullException(nameof(validation));
        }

        public async Task<int> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author();
            author.Name = request.Name;
            author.BirthDate = request.BirthDate;
            author.Nationality = request.Nationality;

            await ValidationAsync(author, cancellationToken);

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await _context.Authors.AddAsync(author, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return author.Id;
            }
            catch (Exception e)
            {
                _logger.LogCritical("Error creating an author: {@Error}", e);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task ValidationAsync(Author entity, CancellationToken cancellationToken)
        {
            var author = await _context
                    .Authors
                    .SingleOrDefaultAsync(
                        a => a.Name == entity.Name && a.Nationality == entity.Nationality && a.BirthDate == entity.BirthDate , cancellationToken);

            if (author != null)
            {
                _validation.Add(
                nameof(entity.Name),
                _localizer["Ya existe este autor"]);
            }

            _validation.AddAndRaiseException(
                nameof(author.Name),
                _localizer["Ya existe un autor con este mismo nombre"]);
        }
    }
}
