using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Skopia.Api.Middleware;
using Skopia.Api.Middleware.Filters;
using Skopia.Application.Contracts;
using Skopia.Application.Mappers;
using Skopia.Application.Services;
using Skopia.Application.Validators;
using Skopia.Infrastructure.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationActionFilter>();
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<ProjectModelValidator>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<CustomSchemaNameFilter>();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://mit-license.org/")
        },
        Title = "Demo API Projects - Skopia",
        Version = "v1",
        Description = "Documenta&#xE7;&#xE3;o oficial da Demo API Projects - Skopia",
        Contact = new OpenApiContact
        {
            Name = "Cezar Lisboa",
            Email = "cezarsystems@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/cezar-lisboa")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskHistoryService, TaskHistoryService>();
builder.Services.AddScoped<ITaskCommentService, TaskCommentService>();
builder.Services.AddScoped<IReportsService, ReportsService>();

builder.Services.AddDbContext<SkopiaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingInterceptor>();
app.UseAuthorization();

// Rodando local
if (app.Environment.IsDevelopment())
{
    var dbPath = Path.Combine(AppContext.BaseDirectory, "Data");
    Directory.CreateDirectory(dbPath);
}

// Vai criar a base se não existir
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SkopiaDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();
app.Run();