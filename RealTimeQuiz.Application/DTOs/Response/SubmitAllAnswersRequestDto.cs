using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.DTOs.Response;

public class SubmitAllAnswersRequestDto
{
    public List<SubmitSingleAnswerRequestDto> Answers { get; set; } = new();
}
