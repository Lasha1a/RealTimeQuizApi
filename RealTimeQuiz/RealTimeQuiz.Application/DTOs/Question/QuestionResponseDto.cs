
namespace RealTimeQuiz.Application.DTOs.Question;

public class QuestionResponseDto
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool IsRequired { get; set; }
    public int TimeLimitSeconds { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AnswerOptionResponseDto> AnswerOptions { get; set; } = new();
}
