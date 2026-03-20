using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.Hubs;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.Responses.Commands;

public record AnnounceResultsCommand(
    Guid QuizId,
    Guid CreatorId) : IRequest<bool>;

public class AnnounceResultsCommandHandler : IRequestHandler<AnnounceResultsCommand, bool>
{
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly IGenericRepository<Response> _responseRepository;
    private readonly IGenericRepository<ResponseAnswer> _responseAnswerRepository;
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IQuizHubService _quizHubService;

    public AnnounceResultsCommandHandler(
        IGenericRepository<Quiz> quizRepository,
        IGenericRepository<Response> responseRepository,
        IGenericRepository<ResponseAnswer> responseAnswerRepository,
        IGenericRepository<Question> questionRepository,
        IQuizHubService quizHubService)
    {
        _quizRepository = quizRepository;
        _responseRepository = responseRepository;
        _responseAnswerRepository = responseAnswerRepository;
        _questionRepository = questionRepository;
        _quizHubService = quizHubService;
    }

    public async Task<bool> Handle(AnnounceResultsCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

        if (quiz == null)
            throw new KeyNotFoundException("Quiz not found");

        // Only creator can announce results
        if (quiz.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to announce results");

        // Get total responses
        var totalResponses = await _responseRepository
            .GetAll()
            .CountAsync(r => r.QuizId == request.QuizId, cancellationToken);

        // Get completed responses
        var completedResponses = await _responseRepository
            .GetAll()
            .CountAsync(r => r.QuizId == request.QuizId
                && r.CompletedAt != null, cancellationToken);

        // Get all questions for this quiz
        var questions = await _questionRepository
            .GetAll()
            .Where(q => q.QuizId == request.QuizId)
            .Include(q => q.AnswerOptions)
            .ToListAsync(cancellationToken);

        // Calculate stats per question
        var questionStats = new List<object>();

        foreach (var question in questions)
        {
            var answers = await _responseAnswerRepository
                .GetAll()
                .Where(ra => ra.QuestionId == question.Id)
                .ToListAsync(cancellationToken);

            var optionStats = question.AnswerOptions.Select(o => new
            {
                OptionId = o.Id,
                OptionText = o.OptionText,
                IsCorrect = o.IsCorrect,
                // Count how many selected this option
                SelectionCount = answers
                    .Count(a => a.SelectedOptionIds.Contains(o.Id))
            });

            questionStats.Add(new
            {
                QuestionId = question.Id,
                QuestionText = question.QuestionText,
                TotalAnswers = answers.Count,
                OptionStats = optionStats
            });
        }

        var results = new
        {
            QuizId = request.QuizId,
            TotalParticipants = totalResponses,
            CompletedParticipants = completedResponses,
            QuestionStats = questionStats
        };

        // Deactivate quiz
        quiz.Deactivate();
        _quizRepository.Update(quiz);
        await _quizRepository.SaveChangesAsync();

        // Broadcast final results to everyone via SignalR
        await _quizHubService.NotifyFinalResults(request.QuizId, results);

        return true;
    }
}
