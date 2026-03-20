using RealTimeQuiz.Domain.Enums;

namespace RealTimeQuiz.Application.DTOs.Question;

public class CreateQuestionRequestDto
{
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; } = QuestionType.Single;
    public int OrderIndex { get; set; }
    public bool IsRequired { get; set; } = true;
    public int TimeLimitSeconds { get; set; } = 30;
    public string? ImageUrl { get; set; }
}
