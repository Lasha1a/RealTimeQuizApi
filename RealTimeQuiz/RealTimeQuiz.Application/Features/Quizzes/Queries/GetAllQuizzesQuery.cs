using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.DTOs.Quiz;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Specifications.Quizzes;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealTimeQuiz.Application.Interfaces.RedisCache;

namespace RealTimeQuiz.Application.Features.Quizzes.Queries;

public record GetAllQuizzesQuery(
    int PageNumber = 1,
    int PageSize = 10) : IRequest<List<QuizResponseDto>>;

public class GetAllQuizzesQueryHandler : IRequestHandler<GetAllQuizzesQuery, List<QuizResponseDto>>
{
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly ICacheService _cacheService;

    public GetAllQuizzesQueryHandler(IGenericRepository<Quiz> quizRepository,  ICacheService cacheService)
    {
        _quizRepository = quizRepository;
        _cacheService = cacheService;
    }

    public async Task<List<QuizResponseDto>> Handle(GetAllQuizzesQuery request,CancellationToken cancellationToken)
    {
        var cacheKey = $"quizzes-page-{request.PageNumber}-{request.PageSize}";
        
        var cached =  await _cacheService.GetAsync<List<QuizResponseDto>>(cacheKey);
        if (cached != null) return cached;  
        
        var spec = new GetAllQuizzesSpec(request.PageNumber, request.PageSize);
        var quizzes = await _quizRepository.GetQueryWithSpec(spec)
            .ToListAsync(cancellationToken);

        var result = quizzes.Select(quiz => new QuizResponseDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Description = quiz.Description,
            Type = quiz.Type.ToString(),
            Visibility = quiz.Visibility.ToString(),
            AccessCode = quiz.AccessCode,
            IsAnonymousAllowed = quiz.IsAnonymousAllowed,
            IsActive = quiz.IsActive,
            StartsAt = quiz.StartsAt,
            EndsAt = quiz.EndsAt,
            CreatedAt = quiz.CreatedAt,
            CreatorUsername = quiz.Creator.Username
        }).ToList();
        
        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(15)); 
        
        return result;
    }
}


