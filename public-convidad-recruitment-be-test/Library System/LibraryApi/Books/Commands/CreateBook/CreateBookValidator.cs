using FluentValidation;

namespace LibraryDatabase.Books.Commands.CreateBook
{
    public class CreateBookValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookValidator()
        {
            RuleFor(v => v.Title).NotEmpty();
            RuleFor(v => v.ISBN).NotEmpty();
            RuleFor(v => v.PublicationDate).NotEmpty();
            RuleFor(v => v.AuthorId).NotEmpty().GreaterThan(0);
        }
    }
}
