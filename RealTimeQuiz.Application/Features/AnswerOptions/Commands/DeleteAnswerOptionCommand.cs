using MediatR;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Features.AnswerOptions.Commands;

public record DeleteAnswerOptionCommand(Guid AnswerOptionId,
    Guid CreatorId) : IRequest<bool>;

public class DeleteAnswerOptionCommandHandler : IRequestHandler<DeleteAnswerOptionCommand, bool>
{
    private readonly IGenericRepository<AnswerOption> _answerOptionRepository;
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IGenericRepository<Quiz> _quizRepository;

    public DeleteAnswerOptionCommandHandler(
        IGenericRepository<AnswerOption> answerOptionRepository,
        IGenericRepository<Question> questionRepository,
        IGenericRepository<Quiz> quizRepository)
    {
        _answerOptionRepository = answerOptionRepository;
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
    }

    public async Task<bool> Handle(DeleteAnswerOptionCommand request, CancellationToken cancellationToken)
    {
        var answerOption = await _answerOptionRepository.GetByIdAsync(request.AnswerOptionId);

        if (answerOption == null)
            throw new KeyNotFoundException("Answer option not found");

        // Check question exists
        var question = await _questionRepository.GetByIdAsync(answerOption.QuestionId);

        // Check only creator can delete options
        var quiz = await _quizRepository.GetByIdAsync(question!.QuizId);

        if (quiz!.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to delete this option");

        _answerOptionRepository.Delete(answerOption);
        await _answerOptionRepository.SaveChangesAsync();

        return true;
    }
}
