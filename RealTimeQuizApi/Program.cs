using FluentValidation.AspNetCore;
using RealTimeQuizApi;
using RealTimeQuizApi.Hubs;
using RealTimeQuizApi.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation(); // for validations

builder.Services.AddTransient<GlobalExceptionHandler>(); //middleware service

builder.Services.AddSignalR(); //added signalr service

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("v1");

builder.Services.AddMainApiDI(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<GlobalExceptionHandler>(); //middleware

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<QuizHub>("/hubs/quiz");

app.Run();
