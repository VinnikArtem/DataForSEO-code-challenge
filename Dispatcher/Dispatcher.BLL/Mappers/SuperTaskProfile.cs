using AutoMapper;
using Dispatcher.BLL.Models;
using Dispatcher.DAL.Entities;

namespace Dispatcher.BLL.Mappers
{
    public class SuperTaskProfile : Profile
    {
        public SuperTaskProfile()
        {
            CreateMap<SuperTask, SuperTaskRequest>()
                .ForMember(
                dest => dest.FileProcessingTasks,
                opt => opt.MapFrom(src => src.FileProcessingTasks.Select(t => new FileProcessingTaskRequest
                {
                    Id = t.Id,
                    LinkToFile = t.LinkToFile,
                    Status = t.Status,
                    SuperTaskId = t.SuperTaskId,
                    HighVolumeKeywordsCount = t.HighVolumeKeywordsCount,
                    MisspelledKeywordsCount = t.MisspelledKeywordsCount,
                    LinesCount = t.LinesCount
                })));
        }
    }
}
