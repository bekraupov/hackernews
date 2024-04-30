using AutoMapper;
using hackernews.Core.Model;
using hackernews.Core.Proxy;

namespace hackernews.Core.Mappers
{
    public class ProxyToModelProfile : Profile
    {
        public ProxyToModelProfile()
        {
            CreateMap<StoryResponse, StoryModel>()
                .ForMember(x => x.Time, a => a.MapFrom(x => DateTime.UnixEpoch.AddSeconds(x.Time)))
                .ForMember(x => x.PostedBy, x => x.MapFrom(nameof(StoryResponse.By)))
                .ForMember(x => x.CommentCount, x => x.MapFrom(nameof(StoryResponse.Descendants)));
        }
    }
}
