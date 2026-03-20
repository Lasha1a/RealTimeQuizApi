using RealTimeQuiz.Application.Specifications.Base;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Specifications.Quizzes;

public class GetQuizWithCreatorSpec : BaseSpecification<Quiz>
{
    public GetQuizWithCreatorSpec(Guid quizId)
        : base(q => q.Id == quizId)
    {
        AddInclude(q => q.Creator);
    }
}
