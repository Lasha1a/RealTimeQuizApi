using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.DTOs.Analytics;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.Analytics.Queries;

public record GetQuizAnalyticsQuery(Guid QuizId) : IRequest<QuizAnalyticsDto>;

public class GetQuizAnalyticsQueryHandler : IRequestHandler<GetQuizAnalyticsQuery, QuizAnalyticsDto>
{
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly IGenericRepository<Response> _responseRepository;
    private readonly IGenericRepository<ResponseAnswer> _responseAnswerRepository;
    private readonly IGenericRepository<Question> _questionRepository;

    public GetQuizAnalyticsQueryHandler(
        IGenericRepository<Quiz> quizRepository,
        IGenericRepository<Response> responseRepository,
        IGenericRepository<ResponseAnswer> responseAnswerRepository,
        IGenericRepository<Question> questionRepository)
    {
        _quizRepository = quizRepository;
        _responseRepository = responseRepository;
        _responseAnswerRepository = responseAnswerRepository;
        _questionRepository = questionRepository;
    }

    public async Task<QuizAnalyticsDto> Handle(GetQuizAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

        if (quiz == null)
            throw new KeyNotFoundException("Quiz not found");

        //get all responses from quiz
        var responses = await _responseRepository
             .GetAll()
             .Where(r => r.QuizId == request.QuizId)
             .ToListAsync(cancellationToken);

        var totalResponses = responses.Count;

        //geet completed responses
        var completedResponses = responses
            .Where(r => r.CompletedAt != null)
            .ToList();

        var completedCount = completedResponses.Count;

        //calculate completion rates
        var completionRate = totalResponses > 0
            ? Math.Round((double)completedCount / totalResponses * 100,2) : 0;

        //calculate average completion time in seconds
        var averageCompletionTime = completedResponses.Any()
            ?Math.Round(completedResponses.Average(r => (r.CompletedAt!.Value - r.StartedAt).TotalSeconds), 2) : 0;

        //get all questions with answer options
        var questions = await _questionRepository
            .GetAll()
            .Where(q => q.QuizId == request.QuizId)
            .Include(q => q.AnswerOptions)
            .OrderBy(q => q.OrderIndex)
            .ToListAsync(cancellationToken);

        //calculate distribution per question
        var questionDistributions = new List<QuestionDistributionDto>();

        foreach(var question in questions)
        {
            var answers = await _responseAnswerRepository
                .GetAll()
                .Where(ra => ra.QuestionId == question.Id)
                .ToListAsync (cancellationToken);

            var totalAnswers = answers.Count;

            var optionDistributions = question.AnswerOptions.Select(o => new OptionDistributionDto
            {
                OptionId = o.Id,
                OptionText = o.OptionText,
                IsCorrect = o.IsCorrect,
                SelectionCount = answers.Count(a => a.SelectedOptionIds.Contains(o.Id)),
                // Calculate percentage of people who picked this option
                Percentage = totalAnswers > 0
                    ? Math.Round((double)answers.Count(a => a.SelectedOptionIds.Contains(o.Id)) / totalAnswers * 100, 2)
                    : 0
            }).ToList();

            questionDistributions.Add(new QuestionDistributionDto
            {
                QuestionId = question.Id,
                QuestionText = question.QuestionText,
                TotalAnswers = totalAnswers,
                OptionDistributions = optionDistributions
            });
        }

        return new QuizAnalyticsDto
        {
            QuizId = request.QuizId,
            TotalResponses = totalResponses,
            CompletedResponses = completedCount,
            CompletionRate = completionRate,
            AverageCompletionTimeSeconds = averageCompletionTime,
            QuestionDistributions = questionDistributions,
            LastCalculatedAt = DateTime.UtcNow
        };
    }
}
