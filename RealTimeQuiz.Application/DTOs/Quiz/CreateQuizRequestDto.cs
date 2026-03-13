using RealTimeQuiz.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.DTOs.Quiz;

public class CreateQuizRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public QuizType Type { get; set; } = QuizType.Quiz;
    public QuizVisibility Visibility { get; set; } = QuizVisibility.Public;
    public string? AccessCode { get; set; } // only needed if private
    public bool IsAnonymousAllowed { get; set; } = false;
    public DateTime? StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
}
