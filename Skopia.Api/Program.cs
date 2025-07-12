using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Skopia.Api.Middleware;
using Skopia.Application.Contracts;
using Skopia.Application.Mappers;
using Skopia.Application.Services;
using Skopia.Application.Validators;
using Skopia.Domain.Contracts;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;
using Skopia.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationActionFilter>();
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<ProjectModelValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IBasicApiOperations<ProjectRequestDTO, ProjectRequestDTO, ProjectResponseDTO, long>, ProjectService>();
builder.Services.AddTransient<IBasicApiOperations<TaskRequestDTO, TaskUpdateRequestDTO, TaskResponseDTO, long>, TaskService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<SkopiaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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