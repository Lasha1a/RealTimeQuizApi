using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Interfaces.Hubs;

public interface IQuizHub
{
    Task AnswerSubmitted(Guid quizId, object stats);
    Task QuizCompleted(Guid quizId);
    Task ParticipantJoined(Guid quizId, int TotalParticiapnts);
    Task QuestionNavigationAsync(Guid quizId, Guid questionId, int questionIndex); //w this creator will move to next question
    Task FinalResultsAnnouncement(Guid quizId, object results);
}
