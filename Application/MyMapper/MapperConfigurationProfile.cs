using AutoMapper;
using Domain.Entities;
using Domain.Requests.Enrollment;

namespace Application.MyMapper
{
    public class MapperConfigurationProfile : Profile
    {
        public MapperConfigurationProfile()
        {
            //User

            //Enrollment
            CreateMap<CreateNewEnrollementRequest, Enrollment>();
        }
    }
}
