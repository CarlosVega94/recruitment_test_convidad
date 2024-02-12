using FluentValidation;

namespace LibraryDatabase.Authors.Commands.CreateAuthor
{
    public class CreateAuthorValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorValidator() {
            RuleFor(v => v.Name).NotEmpty();
            RuleFor(v => v.Nationality).NotEmpty();
            RuleFor(v => v.BirthDate).NotEmpty();
        }
    }
}
