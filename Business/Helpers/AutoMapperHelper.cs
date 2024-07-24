using AutoMapper;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Elasticsearch.Net;

namespace Business.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ReverseMap();
        }
    }
}