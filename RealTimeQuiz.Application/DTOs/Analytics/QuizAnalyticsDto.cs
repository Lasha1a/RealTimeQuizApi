
namespace RealTimeQuiz.Application.DTOs.Analytics;

public class QuizAnalyticsDto
{
    public Guid QuizId { get; set; }
    public int TotalResponses { get; set; }
    public int CompletedResponses { get; set; }
    public double CompletionRate { get; set; }
    public double AverageCompletionTimeSeconds { get; set; }
    public List<QuestionDistributionDto> QuestionDistributions { get; set; } = new();
    public DateTime LastCalculatedAt { get; set; }
}
