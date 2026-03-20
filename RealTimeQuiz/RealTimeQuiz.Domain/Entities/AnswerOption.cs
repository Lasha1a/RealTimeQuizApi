using RealTimeQuiz.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Domain.Entities;

public class AnswerOption : BaseEntity
{
    public Guid QuestionId { get; private set; }
    public string OptionText { get; private set; } = string.Empty;
    public int OrderIndex { get; private set; }
    public bool IsCorrect { get; private set; } = false; //for quiz type

    public Question Question { get; private set; } = null!;
    public IReadOnlyCollection<ResponseAnswer> ResponseAnswers { get; private set; } = new List<ResponseAnswer>();

    private AnswerOption() { }

    public static AnswerOption Create(
        Guid questionId,
        string optionText,
        int orderIndex,
        bool isCorrect = false)
    {
        return new AnswerOption
        {
            QuestionId = questionId,
            OptionText = optionText,
            OrderIndex = orderIndex,
            IsCorrect = isCorrect
        };
    }

    public void UpdateDetails(string optionText, bool isCorrect)
    {
        OptionText = optionText;
        IsCorrect = isCorrect;
    }

    public void UpdateOrder(int orderIndex) => OrderIndex = orderIndex;
    public void MarkAsCorrect() => IsCorrect = true;
    public void MarkAsIncorrect() => IsCorrect = false;
}
