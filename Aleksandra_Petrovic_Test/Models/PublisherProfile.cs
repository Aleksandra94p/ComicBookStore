using Aleksandra_Petrovic_Test.Models.DTO;
using AutoMapper;

namespace Aleksandra_Petrovic_Test.Models
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<Publisher, PublisherDTO>();
        }
    }
}
