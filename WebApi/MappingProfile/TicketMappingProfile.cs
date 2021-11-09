using AutoMapper;
using Core.Models;
using PlatformDemo.DTO;

namespace PlatformDemo.MappingProfile
{
    public class TicketMappingProfile : Profile
    {
        public TicketMappingProfile()
        {
            CreateMap<Ticket, TicketDTO>()
                .ForMember(x => x.Id,
                    o => 
                        o.MapFrom(src => src.ProjectId));
        }
    }
}