
using FluentValidation;
using RealTimeQuiz.Application.DTOs.Question;

namespace RealTimeQuiz.Application.Validators.Question;

public class UpdateQuestionRequestValidator : AbstractValidator<UpdateQuestionRequestDto>
{
    public UpdateQuestionRequestValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty().WithMessage("Question text is required")
            .MinimumLength(3).WithMessage("Question must be at least 3 characters")
            .MaximumLength(1000).WithMessage("Question cannot exceed 1000 characters");

        RuleFor(x => x.QuestionType)
            .IsInEnum().WithMessage("Invalid question type");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Order index must be 0 or greater");

        RuleFor(x => x.TimeLimitSeconds)
            .GreaterThanOrEqualTo(0).WithMessage("Time limit must be 0 or greater")
            .LessThanOrEqualTo(300).WithMessage("Time limit cannot exceed 300 seconds");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters")
            .When(x => x.ImageUrl != null);
    }
}
