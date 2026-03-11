using RealTimeQuiz.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Domain.Entities;

public class ResponseAnswer : BaseEntity
{
    public Guid ResponseId { get; private set; }
    public Guid QuestionId { get; private set; }
    public List<Guid> SelectedOptionIds { get; private set; } = new List<Guid>();
    public string? TextAnswer { get; private set; }
    public int? RatingValue { get; private set; }
    public DateTime AnsweredAt { get; private set; } = DateTime.UtcNow;

    // Navigation properties
    public Response Response { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    private ResponseAnswer() { }

    public static ResponseAnswer Create(
        Guid responseId,
        Guid questionId,
        List<Guid>? selectedOptionIds = null,
        string? textAnswer = null,
        int? ratingValue = null)
    {
        return new ResponseAnswer
        {
            ResponseId = responseId,
            QuestionId = questionId,
            SelectedOptionIds = selectedOptionIds ?? new List<Guid>(),
            TextAnswer = textAnswer,
            RatingValue = ratingValue
        };
    }
}
