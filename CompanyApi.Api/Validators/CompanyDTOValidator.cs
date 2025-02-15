using CompanyApi.Api.Models.DTO;
using FluentValidation;

namespace CompanyApi.Api.Validators
{
    public class CompanyDTOValidator : AbstractValidator<CompanyDTO>
    {
        public CompanyDTOValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Company name is required.");
            RuleFor(c => c.Exchange).NotEmpty().WithMessage("Exchange is required.");
            RuleFor(c => c.Ticker).NotEmpty().WithMessage("Stock ticker is required.");
            RuleFor(c => c.Isin)
                .NotEmpty().WithMessage("ISIN is required.")
                .Matches("^[A-Z]{2}.*$").WithMessage("ISIN must start with two uppercase letters.")
                .MaximumLength(12).WithMessage("ISIN must be of Max 12 letters only");
        }
    }
}
