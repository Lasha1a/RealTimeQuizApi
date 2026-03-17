using FluentValidation;
using RealTimeQuiz.Application.DTOs.Response;

namespace RealTimeQuiz.Application.Validators.Response;

public class StartResponseRequestValidator : AbstractValidator<StartResponseRequestDto>
{
    public StartResponseRequestValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("Quiz ID is required");
    }
}
