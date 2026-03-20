using Microsoft.AspNetCore.SignalR;
using RealTimeQuiz.Application.Interfaces.Hubs;

namespace RealTimeQuiz.Infrastructure.Services.Hubs;

public class QuizHub : Hub<IQuizHub>
{ 
    //player or creator joins specific room
    public async Task JoinQuiz(string quizId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"quiz - {quizId}");
    }

    //player or creator leaves the room 
    public async Task LeaveQuiz(string quizId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"quiz-{quizId}");
    }
}
