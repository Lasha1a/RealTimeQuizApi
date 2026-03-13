using MediatR;
using RealTimeQuiz.Application.DTOs.Quiz;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;
using RealTimeQuiz.Application.Specifications.Quizzes;

namespace RealTimeQuiz.Application.Features.Quizzes.Queries;

public record GetQuizByIdQuery(Guid QuizId) : IRequest<QuizResponseDto>;

public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, QuizResponseDto>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public GetQuizByIdQueryHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<QuizResponseDto> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
    {
        // Use spec to include Creator so we can get username
        var spec = new GetQuizWithCreatorSpec(request.QuizId);
        var quiz = await _quizRepository.GetEntityWithSpec(spec);

        if (quiz == null)
            throw new Exception("Quiz not found");

        return new QuizResponseDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Description = quiz.Description,
            Type = quiz.Type.ToString(),
            Visibility = quiz.Visibility.ToString(),
            AccessCode = quiz.AccessCode,
            IsAnonymousAllowed = quiz.IsAnonymousAllowed,
            IsActive = quiz.IsActive,
            StartsAt = quiz.StartsAt,
            EndsAt = quiz.EndsAt,
            CreatedAt = quiz.CreatedAt,
            CreatorUsername = quiz.Creator.Username
        };
    }
}
