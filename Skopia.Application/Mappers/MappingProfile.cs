using AutoMapper;
using Skopia.Application.Converters;
using Skopia.Domain.Enums;
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
                .ForMember(dest => dest.Status, opt => opt.MapFrom(
                    src => ToolsServiceExtension.GetEnumDescription<StatusEnum>(src.Status)))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(
                    src => ToolsServiceExtension.GetEnumDescription<PriorityEnum>(src.Priority)))
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(
                    src => src.Comments.Select(c => c.Content).ToArray()));

            CreateMap<TaskRequestDTO, TaskModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => char.ToUpperInvariant(src.Status)))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => char.ToUpperInvariant(src.Priority)))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(
                    src => DateConverter.Parse(src.ExpirationDate)));

            CreateMap<ProjectModel, ProjectResponseDTO>()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.NumberOfTasks, opt => opt.MapFrom(src => src.Tasks.Count));

            CreateMap<ProjectModel, ProjectBasicInfoResponseDTO>()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.User));

            CreateMap<ProjectRequestDTO, ProjectModel>();

            CreateMap<UserModel, UserInfoResponseDTO>();

            CreateMap<TaskHistoryModel, TaskHistoryModel>();
        }
    }
}