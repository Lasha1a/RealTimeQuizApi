using FluentValidation;
using RealTimeQuiz.Application.DTOs.Quiz;
using RealTimeQuiz.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Validators.Quiz;

public class UpdateQuizRequestValidator : AbstractValidator<UpdateQuizRequestDto>
{
    public UpdateQuizRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Visibility)
            .IsInEnum().WithMessage("Invalid visibility type");

        RuleFor(x => x.AccessCode)
            .NotEmpty().WithMessage("Access code is required for private quizzes")
            .When(x => x.Visibility == QuizVisibility.Private);

        RuleFor(x => x.EndsAt)
            .GreaterThan(x => x.StartsAt)
            .WithMessage("End date must be after start date")
            .When(x => x.StartsAt.HasValue && x.EndsAt.HasValue);
    }
}
