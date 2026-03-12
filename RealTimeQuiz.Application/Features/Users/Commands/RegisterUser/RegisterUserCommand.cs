using MediatR;
using RealTimeQuiz.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<UserResponseDto>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
  
}
