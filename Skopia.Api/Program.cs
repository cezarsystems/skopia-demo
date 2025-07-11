using FluentValidation;
using Skopia.Api.Middleware;
using Skopia.Application.Services;
using Skopia.Application.Validators;
using Skopia.Domain.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationActionFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<ProjectModelValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IBasicApiOperations<ProjectRequestDTO, ProjectRequestDTO, ProjectResponseDTO, long>, ProjectService>();
builder.Services.AddTransient<IBasicApiOperations<TaskRequestDTO, TaskUpdateRequestDTO, TaskResponseDTO, long>, TaskService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ErrorHandlingInterceptor>();
app.Run();