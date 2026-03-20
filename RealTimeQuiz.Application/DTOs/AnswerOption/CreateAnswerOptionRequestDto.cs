
namespace RealTimeQuiz.Application.DTOs.AnswerOption;

public class CreateAnswerOptionRequestDto
{
    public string OptionText { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool IsCorrect { get; set; } = false;
}
