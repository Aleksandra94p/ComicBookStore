using Aleksandra_Petrovic_Test.Models.DTO;
using AutoMapper;

namespace Aleksandra_Petrovic_Test.Models
{
    public class ComicBookProfile : Profile
    {
        public ComicBookProfile()
        {
             CreateMap<ComicBook, ComicBookDTO>().ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher.Name));
        }
    }
}
