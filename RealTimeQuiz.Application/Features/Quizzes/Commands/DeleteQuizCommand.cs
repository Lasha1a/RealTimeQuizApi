using MediatR;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Features.Quizzes.Commands;

public record DeleteQuizCommand(
    Guid QuizId,
    Guid CreatorId) : IRequest<bool>;

public class DeleteQuizCommandHandler : IRequestHandler<DeleteQuizCommand, bool>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public DeleteQuizCommandHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<bool> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

        if (quiz == null)
            throw new Exception("Quiz not found");

        // Make sure only the creator can delete the quiz
        if (quiz.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to delete this quiz");

        _quizRepository.Delete(quiz);
        await _quizRepository.SaveChangesAsync();

        return true;
    }
}
