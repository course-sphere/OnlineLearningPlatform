using AutoMapper;
using Domain.Entities;
using Domain.Requests.Course;
using Domain.Requests.Enrollment;
using Domain.Requests.Lesson;
using Domain.Responses.Course;

namespace Application.MyMapper
{
    public class MapperConfigurationProfile : Profile
    {
        public MapperConfigurationProfile()
        {
            //User

            //Enrollment
            CreateMap<CreateNewEnrollementRequest, Enrollment>();

            //Course
            CreateMap<CreateNewCourseRequest, Course>();
            CreateMap<Course, CourseResponse>();
            CreateMap<UpdateCourseRequest, Course>();
            //Lesson
            CreateMap<CreateNewLessonRequest, Lesson>();
        }
    }
}
