using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.DTOs.Question;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Specifications.Questions;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.Questions.Queries;

public record GetQuestionsByQuizQuery(Guid QuizId) : IRequest<List<QuestionResponseDto>>;

public class GetQuestionsByQuizQueryHandler : IRequestHandler<GetQuestionsByQuizQuery, List<QuestionResponseDto>>
{
    private readonly IGenericRepository<Question> _questionRepository;

    public GetQuestionsByQuizQueryHandler(IGenericRepository<Question> questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<List<QuestionResponseDto>> Handle(GetQuestionsByQuizQuery request, CancellationToken cancellationToken)
    {
        // Use spec to include AnswerOptions and order by OrderIndex
        var spec = new GetQuestionsByQuizSpec(request.QuizId);
        var questions = await _questionRepository
            .GetQueryWithSpec(spec)
            .ToListAsync(cancellationToken);

        return questions.Select(q => new QuestionResponseDto
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
    }
}
