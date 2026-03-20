using RealTimeQuiz.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Domain.Entities;

public class QuizAnalytics : BaseEntity
{
    public Guid QuizId { get; private set; }
    public int TotalResponses { get; private set; }
    public double AverageCompletionTime { get; private set; }
    public double CompletionRate { get; private set; }
    public DateTime LastCalculatedAt { get; private set; } = DateTime.UtcNow;

    // Navigation properties
    public Quiz Quiz { get; private set; } = null!;

    private QuizAnalytics() { }

    public static QuizAnalytics Create(Guid quizId)
    {
        return new QuizAnalytics
        {
            QuizId = quizId,
            TotalResponses = 0,
            AverageCompletionTime = 0,
            CompletionRate = 0
        };
    }

    public void UpdateAnalytics(int totalResponses, double averageCompletionTime, double completionRate)
    {
        TotalResponses = totalResponses;
        AverageCompletionTime = averageCompletionTime;
        CompletionRate = completionRate;
        LastCalculatedAt = DateTime.UtcNow;
    }
}
