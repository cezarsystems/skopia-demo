using FluentValidation;
using Skopia.Application.Contracts;
using Skopia.DTOs.Models.Request;

namespace Skopia.Application.Validators
{
    public class TaskUpdateModelValidator : AbstractValidator<TaskUpdateRequestDTO>
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        public TaskUpdateModelValidator(ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
        }
    }
}