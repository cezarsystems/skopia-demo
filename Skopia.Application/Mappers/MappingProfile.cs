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
                    src => string.IsNullOrEmpty(src.Comments)
                    ? Array.Empty<string>()
                    : src.Comments.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)));

            CreateMap<ProjectModel, ProjectResponseDTO>()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.NumberOfTasks, opt => opt.MapFrom(src => src.Tasks.Count));

            CreateMap<ProjectModel, ProjectBasicInfoResponseDTO>()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.User));

            CreateMap<ProjectRequestDTO, ProjectModel>();

            CreateMap<TaskRequestDTO, TaskModel>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => new[] { src.Comment }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => char.ToUpperInvariant(src.Status)))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => char.ToUpperInvariant(src.Priority)))
                .ForMember(dest => dest.ExpirationData, opt => opt.MapFrom(
                    src => DateConverter.Parse(src.ExpirationDate)));

            CreateMap<UserModel, UserInfoResponseDTO>();

            CreateMap<TaskHistoryEntryModel, TaskHistoryModel>()
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}