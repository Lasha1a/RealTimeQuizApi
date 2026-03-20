using RealTimeQuiz.Application.Specifications.Base;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Specifications.Questions;

public class GetQuestionsByQuizSpec : BaseSpecification<Question>
{
    public GetQuestionsByQuizSpec(Guid quizId)
        : base(q => q.QuizId == quizId)
    {
        AddInclude(q => q.AnswerOptions);
        AddOrderBy(q => q.OrderIndex);
    }
}
