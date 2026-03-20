using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.DTOs.Question;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.RedisCache;
using RealTimeQuiz.Application.Specifications.Questions;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.Questions.Queries;

public record GetQuestionsByQuizQuery(Guid QuizId) : IRequest<List<QuestionResponseDto>>;

public class GetQuestionsByQuizQueryHandler : IRequestHandler<GetQuestionsByQuizQuery, List<QuestionResponseDto>>
{
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly ICacheService _cacheService;

    public GetQuestionsByQuizQueryHandler(IGenericRepository<Question> questionRepository,  ICacheService cacheService)
    {
        _questionRepository = questionRepository;
        _cacheService = cacheService;
    }

    public async Task<List<QuestionResponseDto>> Handle(GetQuestionsByQuizQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"quiz-questions-{request.QuizId}";
        var cached = await _cacheService.GetAsync<List<QuestionResponseDto>>(cacheKey);
        if (cached != null) return cached;
        
        // Use spec to include AnswerOptions and order by OrderIndex
        var spec = new GetQuestionsByQuizSpec(request.QuizId);
        var questions = await _questionRepository
            .GetQueryWithSpec(spec)
            .ToListAsync(cancellationToken);

        var result = questions.Select(q => new QuestionResponseDto
        {
            Id = q.Id,
            QuizId = q.QuizId,
            QuestionText = q.QuestionText,
            QuestionType = q.QuestionType.ToString(),
            OrderIndex = q.OrderIndex,
            IsRequired = q.IsRequired,
            TimeLimitSeconds = q.TimeLimitSeconds,
            ImageUrl = q.ImageUrl,
            CreatedAt = q.CreatedAt,
            AnswerOptions = q.AnswerOptions.Select(a => new AnswerOptionResponseDto
            {
                Id = a.Id,
                OptionText = a.OptionText,
                OrderIndex = a.OrderIndex,
                IsCorrect = a.IsCorrect
            }).ToList()
        }).ToList();
        
        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromHours(1));

        return result;
    }
}
