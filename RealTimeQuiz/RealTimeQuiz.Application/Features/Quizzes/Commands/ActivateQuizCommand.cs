using MediatR;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Features.Quizzes.Commands;

public record ActivateQuizCommand(
    Guid QuizId,
    Guid CreatorId,
    bool IsActive) : IRequest<bool>;

public class ActivateQuizCommandHandler : IRequestHandler<ActivateQuizCommand, bool>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public ActivateQuizCommandHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<bool> Handle(ActivateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

        if (quiz == null)
            throw new Exception("Quiz not found");

        // Only creator can activate/deactivate
        if (quiz.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to modify this quiz");

        if (request.IsActive)
            quiz.Activate();
        else
            quiz.Deactivate();

        _quizRepository.Update(quiz);
        await _quizRepository.SaveChangesAsync();

        return true;
    }
}
