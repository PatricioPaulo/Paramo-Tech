using AutoMapper;
using Sat.Recruitment.Api.DTO;
using Sat.Recruitment.Api.Entities;

namespace Sat.Recruitment.Api.Utils
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
