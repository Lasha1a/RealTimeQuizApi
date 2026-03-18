using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Interfaces.Hubs;

public interface IQuizHubService
{
    Task NotifyParticipantJoined(Guid quizId, int totalParticipants);
    Task NotifyAnswerSubmitted(Guid quizId, object stats);
    Task NotifyQuizCompleted(Guid quizId);
}
