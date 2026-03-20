
using RealTimeQuiz.Domain.Enums;

namespace RealTimeQuiz.Application.DTOs.Question;

public class UpdateQuestionRequestDto
{
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; }
    public int OrderIndex { get; set; }
    public bool IsRequired { get; set; }
    public int TimeLimitSeconds { get; set; }
    public string? ImageUrl { get; set; }
}
