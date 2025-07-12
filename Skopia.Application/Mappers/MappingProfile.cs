using AutoMapper;
using Skopia.Domain.Models;
using Skopia.DTOs.Models.Request;
using Skopia.DTOs.Models.Response;

namespace Skopia.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskModel, TaskResponseDTO>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project));

            CreateMap<ProjectModel, ProjectResponseDTO>();

            CreateMap<ProjectModel, ProjectBasicInfoResponseDTO>();

            CreateMap<ProjectRequestDTO, ProjectModel>();

            CreateMap<ProjectModel, ProjectResponseDTO>()
                .ForMember(dest => dest.NumberOfTasks, opt => opt.MapFrom(src => src.Tasks.Count));

            CreateMap<TaskRequestDTO, TaskModel>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => new[] { src.Comment }));
        }
    }
}