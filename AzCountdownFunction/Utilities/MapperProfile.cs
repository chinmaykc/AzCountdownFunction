using AutoMapper;
using FuncCountdown.DTOs;

namespace FuncCountdown.Utilities
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<EventDetails, EventDetailsDto>();

            CreateMap<EventDetailsEntity, EventDetails>()
                .ForMember(dest => dest.UserID,
                    opt => opt.MapFrom(source => source.nUserID))
                .ForMember(dest => dest.EventName,
                    opt => opt.MapFrom(source => source.szEventName))
                .ForMember(dest => dest.EventDate,
                    opt => opt.MapFrom(source => source.szEventDate));
        }
    }
}
