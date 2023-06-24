using FluentValidation;

namespace Application.Files.Queries.ExportPdf
{
    public class ExportPdfQueryValidator : AbstractValidator<ExportPdfModel>
    {
        public ExportPdfQueryValidator()
        {
            RuleFor(x => x.Template)
                .NotEmpty()
                .WithMessage("Template is required");
        }
    }
}
