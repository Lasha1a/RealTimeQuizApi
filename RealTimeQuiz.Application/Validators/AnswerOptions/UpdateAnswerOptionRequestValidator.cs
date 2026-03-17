using FluentValidation;
using RealTimeQuiz.Application.DTOs.AnswerOption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Validators.AnswerOptions; 

public class UpdateAnswerOptionRequestValidator : AbstractValidator<UpdateAnswerOptionRequestDto>
{
    public UpdateAnswerOptionRequestValidator()
    {
        RuleFor(x => x.OptionText)
            .NotEmpty().WithMessage("Option text is required")
            .MinimumLength(1).WithMessage("Option text must be at least 1 character")
            .MaximumLength(500).WithMessage("Option text cannot exceed 500 characters");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Order index must be 0 or greater");
    }
}
