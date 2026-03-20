using MediatR;
using RealTimeQuiz.Application.DTOs.Quiz;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;
using RealTimeQuiz.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Features.Quizzes.Commands;

public record CreateQuizCommand(
    Guid CreatorId,
    string Title,
    string Description,
    QuizType Type,
    QuizVisibility Visibility,
    string? AccessCode,
    bool IsAnonymousAllowed,
    DateTime? StartsAt,
    DateTime? EndsAt) : IRequest<QuizResponseDto>;

public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, QuizResponseDto>
{
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly IGenericRepository<User> _userRepository;

    public CreateQuizCommandHandler(
        IGenericRepository<Quiz> quizRepository,
        IGenericRepository<User> userRepository)
    {
        _quizRepository = quizRepository;
        _userRepository = userRepository;
    }

    public async Task<QuizResponseDto> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var creator = await _userRepository.GetByIdAsync(request.CreatorId);
        if(creator == null)
        {
            throw new KeyNotFoundException("Creator not found");
        }

        var quiz = Quiz.Create(
            request.CreatorId,
            request.Title,
            request.Description,
            request.AccessCode ?? string.Empty,
            request.IsAnonymousAllowed,
            request.StartsAt ?? DateTime.UtcNow,
            request.EndsAt ?? DateTime.UtcNow.AddDays(7),
            true);

        await _quizRepository.AddAsync(quiz);
        await _quizRepository.SaveChangesAsync();

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
            CreatorUsername = creator.Username
        };
    }
}

