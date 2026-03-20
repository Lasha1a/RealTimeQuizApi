using Microsoft.AspNetCore.SignalR;
using RealTimeQuiz.Application.Interfaces.Hubs;

namespace RealTimeQuiz.Infrastructure.Services.Hubs;

public class QuizHubService : IQuizHubService
{
    private readonly IHubContext<QuizHub, IQuizHub> _hubContext;

    public QuizHubService(IHubContext<QuizHub, IQuizHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyParticipantJoined(Guid quizId, int totalParticipnts) =>
        await _hubContext.Clients
            .Group($"quiz-{quizId}")
            .ParticipantJoined(quizId, totalParticipnts);

    public async Task NotifyAnswerSubmitted(Guid quizId, object stats) =>
        await _hubContext.Clients
            .Group($"quiz-{quizId}")
            .AnswerSubmitted(quizId, stats);

    public async Task NotifyQuizCompleted(Guid quizId)
        => await _hubContext.Clients
            .Group($"quiz-{quizId}")
            .QuizCompleted(quizId);

    public async Task NotifyQuestionNavigation(Guid quizId,Guid questionId, int questionIndex) =>
        await _hubContext.Clients
            .Group($"quiz - {quizId}")
            .QuestionNavigationAsync(quizId, questionId, questionIndex);

    public async Task NotifyFinalResults(Guid quizId,object results) =>
        await _hubContext.Clients
            .Group($"quiz-{quizId}")
            .FinalResultsAnnouncement(quizId, results);
}
