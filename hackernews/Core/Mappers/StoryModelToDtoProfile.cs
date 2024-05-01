using AutoMapper;
using hackernews.Core.Dto;
using hackernews.Core.Model;
using hackernews.Core.Proxy;

namespace hackernews.Core.Mappers
{
    public class StoryModelToDtoProfile : Profile
    {
        public StoryModelToDtoProfile()
        {
            CreateMap<StoryModel, StoryDto>();
        }
    }
}
