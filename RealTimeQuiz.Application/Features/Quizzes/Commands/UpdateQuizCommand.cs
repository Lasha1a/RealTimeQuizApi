using MediatR;
using RealTimeQuiz.Application.DTOs.Quiz;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.RedisCache;
using RealTimeQuiz.Domain.Entities;
using RealTimeQuiz.Domain.Enums;

namespace RealTimeQuiz.Application.Features.Quizzes.Commands;

public record UpdateQuizCommand(
    Guid QuizId,
    Guid CreatorId,
    string Title,
    string Description,
    QuizVisibility Visibility,
    string? AccessCode,
    bool IsAnonymousAllowed,
    DateTime? StartsAt,
    DateTime? EndsAt) : IRequest<QuizResponseDto>;

public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, QuizResponseDto>
{
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly ICacheService _cacheService;

    public UpdateQuizCommandHandler(IGenericRepository<Quiz> quizRepository,  ICacheService cacheService)
    {
        _quizRepository = quizRepository;
        _cacheService = cacheService;
    }

    public async Task<QuizResponseDto> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);
        if (quiz == null)
        {
            throw new KeyNotFoundException("quiz not found");
        }

        //making sure only creator can update the quiz 
        if(quiz.CreatorId != request.CreatorId)
        {
            throw new UnauthorizedAccessException("unothorized to update quiz");
        }

        quiz.UpdateDetails(request.Title, request.Description);
        quiz.UpdateSchedule(
            request.StartsAt ?? quiz.StartsAt,
            request.EndsAt ?? quiz.EndsAt);

        if (request.Visibility == QuizVisibility.Public)
            quiz.MakePublic();
        else
            quiz.MakePrivate();

        if (request.IsAnonymousAllowed)
            quiz.AllowAnonymous();
        else
            quiz.DisallowAnonymous();

        _quizRepository.Update(quiz);
        await _quizRepository.SaveChangesAsync();
        
        await _cacheService.RemoveAsync($"quiz-{request.QuizId}");

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
            CreatedAt = quiz.CreatedAt
        };
    }
}
