using AutoMapper;
using Dispatcher.BLL.Models;
using Dispatcher.DAL.Entities;

namespace Dispatcher.BLL.Mappers
{
    public class FileProcessingTaskProfile : Profile
    {
        public FileProcessingTaskProfile()
        {
            CreateMap<FileProcessingTask, FileProcessingTaskRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LinkToFile, opt => opt.MapFrom(src => src.LinkToFile))
                .ForMember(dest => dest.LinesCount, opt => opt.MapFrom(src => src.LinesCount))
                .ForMember(dest => dest.HighVolumeKeywordsCount, opt => opt.MapFrom(src => src.HighVolumeKeywordsCount))
                .ForMember(dest => dest.MisspelledKeywordsCount, opt => opt.MapFrom(src => src.MisspelledKeywordsCount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.SuperTaskId, opt => opt.MapFrom(src => src.SuperTaskId))
                .ForMember(dest => dest.IsFileCorrupted, opt => opt.MapFrom(src => src.IsFileCorrupted))
                .ForMember(dest => dest.InvalidLines, opt => opt.MapFrom(src => src.InvalidLines.Select(t => t.LineNumber)));

            CreateMap<FileProcessingTaskRequest, FileProcessingTask>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LinkToFile, opt => opt.MapFrom(src => src.LinkToFile))
                .ForMember(dest => dest.LinesCount, opt => opt.MapFrom(src => src.LinesCount))
                .ForMember(dest => dest.HighVolumeKeywordsCount, opt => opt.MapFrom(src => src.HighVolumeKeywordsCount))
                .ForMember(dest => dest.MisspelledKeywordsCount, opt => opt.MapFrom(src => src.MisspelledKeywordsCount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.SuperTaskId, opt => opt.MapFrom(src => src.SuperTaskId))
                .ForMember(dest => dest.IsFileCorrupted, opt => opt.MapFrom(src => src.IsFileCorrupted))
                .ForMember(dest => dest.InvalidLines, opt => opt.MapFrom(src => src.InvalidLines.Select(t => new InvalidLine
                {
                    LineNumber = t,
                    FileProcessingTaskId = src.Id
                })));
        }
    }
}
