using FluentValidation;

namespace LibraryDatabase.Books.Queries.GetBooksById
{
    public class GetBooksByIdValidator : AbstractValidator<GetBooksByIdQuery>
    {
        public GetBooksByIdValidator() 
        {
            RuleFor(v => v.Id).GreaterThan(0);
        }
    }
}
