
namespace RealTimeQuiz.Application.DTOs.Question;

public class AnswerOptionResponseDto
{
    public Guid Id { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool IsCorrect { get; set; }
}
