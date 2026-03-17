using FluentValidation;
using RealTimeQuiz.Application.DTOs.Response;

namespace RealTimeQuiz.Application.Validators.Response;

public class SubmitSingleAnswerRequestValidator : AbstractValidator<SubmitSingleAnswerRequestDto>
{
    public SubmitSingleAnswerRequestValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("Question ID is required");

        // At least one answer type must be provided
        RuleFor(x => x)
            .Must(x => x.SelectedOptionIds.Any() ||
                       !string.IsNullOrEmpty(x.TextAnswer) ||
                       x.RatingValue.HasValue)
            .WithMessage("At least one answer must be provided");

        RuleFor(x => x.RatingValue)
            .InclusiveBetween(1, 10).WithMessage("Rating must be between 1 and 10")
            .When(x => x.RatingValue.HasValue);
    }
}
