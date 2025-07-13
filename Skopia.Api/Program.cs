using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Skopia.Api.Middleware;
using Skopia.Application.Contracts;
using Skopia.Application.Mappers;
using Skopia.Application.Services;
using Skopia.Application.Validators;
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

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskHistoryService, TaskHistoryService>();
builder.Services.AddScoped<ITaskCommentService, TaskCommentService>();

builder.Services.AddScoped<IGetOperations<ProjectResponseDTO, long>, ProjectService>();
builder.Services.AddScoped<IPostOperations<ProjectRequestDTO, ProjectResponseDTO>, ProjectService>();
builder.Services.AddScoped<IDeleteOperations<long>, ProjectService>();

builder.Services.AddDbContext<SkopiaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingInterceptor>();
app.UseAuthorization();

app.MapControllers();
app.Run();