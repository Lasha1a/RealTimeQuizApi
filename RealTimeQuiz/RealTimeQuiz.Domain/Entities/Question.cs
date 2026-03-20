using RealTimeQuiz.Domain.Common;
using RealTimeQuiz.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Domain.Entities;

public class Question :  BaseEntity
{
    public Guid QuizId { get; private set; }
    public string QuestionText { get; private set; } = string.Empty;
    public QuestionType QuestionType { get; private set; } = QuestionType.Single;
    public int OrderIndex { get; private set; }
    public bool IsRequired { get; private set; } = true;
    public int TimeLimitSeconds { get; private set; } = 30;
    public string? ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Navigation properties
    public Quiz Quiz { get; private set; } = null!;
    public IReadOnlyCollection<AnswerOption> AnswerOptions { get; private set; } = new List<AnswerOption>();
    public IReadOnlyCollection<ResponseAnswer> ResponseAnswers { get; private set; } = new List<ResponseAnswer>();

    private Question() { }

    public static Question Create(
        Guid quizId,
        string questionText,
        QuestionType questionType,
        int orderIndex,
        bool isRequired,
        int timeLimitSeconds,
        string? imageUrl = null)
    {
        return new Question
        {
            QuizId = quizId,
            QuestionText = questionText,
            QuestionType = questionType,
            OrderIndex = orderIndex,
            IsRequired = isRequired,
            TimeLimitSeconds = timeLimitSeconds,
            ImageUrl = imageUrl
        };
    }

    public void UpdateDetails(string questionText, QuestionType questionType, int timeLimitSeconds, string? imageUrl)
    {
        QuestionText = questionText;
        QuestionType = questionType;
        TimeLimitSeconds = timeLimitSeconds;
        ImageUrl = imageUrl;
    }

    public void UpdateOrder(int orderIndex) => OrderIndex = orderIndex;
    public void MakeRequired() => IsRequired = true;
    public void MakeOptional() => IsRequired = false;

}
