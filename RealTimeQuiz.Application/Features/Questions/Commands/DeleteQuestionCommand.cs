using MediatR;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Features.Questions.Commands;

public record DeleteQuestionCommand(
    Guid QuestionId,
    Guid CreatorId) : IRequest<bool>;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, bool>
{
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IGenericRepository<Quiz> _quizRepository;

    public DeleteQuestionCommandHandler(
        IGenericRepository<Question> questionRepository,
        IGenericRepository<Quiz> quizRepository)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
    }

    public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.QuestionId);

        if (question == null)
            throw new KeyNotFoundException("Question not found");

        // Check quiz ownership
        var quiz = await _quizRepository.GetByIdAsync(question.QuizId);

        if (quiz!.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to delete this question");

        _questionRepository.Delete(question);
        await _questionRepository.SaveChangesAsync();

        return true;
    }
}
