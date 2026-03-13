using RealTimeQuiz.Application.Specifications.Base;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Specifications.Quizzes;

public class GetAllQuizzesSpec : BaseSpecification<Quiz>
{
    public GetAllQuizzesSpec(int pageNumber, int pageSize)
    {
        AddInclude(q => q.Creator);
        AddOrderByDescending(q => q.CreatedAt);
        ApplyPaging((pageNumber - 1) * pageSize, pageSize);
    }
}
