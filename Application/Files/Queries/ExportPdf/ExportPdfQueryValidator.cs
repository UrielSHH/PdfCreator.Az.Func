using FluentValidation;

namespace Application.Files.Queries.ExportPdf
{
    public class ExportPdfQueryValidator : AbstractValidator<ExportPdfModel>
    {
        public ExportPdfQueryValidator()
        {
            RuleFor(x => x.Template)
                .NotEmpty().WithMessage("Template is required")
                .Must(BeHtml).WithMessage("Template must be Html");
        }

        private static bool BeHtml(string template) => template.StartsWith("<html") && template.EndsWith("</html>");
    }
}
