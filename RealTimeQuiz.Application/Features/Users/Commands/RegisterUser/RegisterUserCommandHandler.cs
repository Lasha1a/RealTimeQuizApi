using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.DTOs.User;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.PasswordHash;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResponseDto>
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IGenericRepository<User> userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }



    public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository
            .GetAll()
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if(existingUser != null)
        {
            throw new Exception("Email already in use");
        }

        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        var user = User.Create(request.Username, request.Email, hashedPassword);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}
