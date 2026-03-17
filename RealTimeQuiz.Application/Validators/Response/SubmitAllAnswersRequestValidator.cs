using FluentValidation;
using RealTimeQuiz.Application.DTOs.Response;

namespace RealTimeQuiz.Application.Validators.Response;

public class SubmitAllAnswersRequestValidator : AbstractValidator<SubmitAllAnswersRequestDto>
{
    public SubmitAllAnswersRequestValidator()
    {
        RuleFor(x => x.Answers)
            .NotEmpty().WithMessage("Answers list cannot be empty");

        RuleForEach(x => x.Answers)
            .SetValidator(new SubmitSingleAnswerRequestValidator());
    }
}
