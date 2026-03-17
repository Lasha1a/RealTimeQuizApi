using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.DTOs.Response;

public class StartResponseRequestDto
{
    public Guid QuizId { get; set; }
    public string? SessionId { get; set; }
}
