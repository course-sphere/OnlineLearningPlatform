using AutoMapper;
using Domain.Entities;
using Domain.Requests.Course;
using Domain.Requests.Enrollment;
using Domain.Requests.Lesson;
using Domain.Requests.LessonResource;
using Domain.Requests.Module;
using Domain.Responses.Course;
using Domain.Responses.Lesson;
using Domain.Responses.LessonResource;
using Domain.Responses.Module;

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
            CreateMap<Course, GetAllCourseForAdminResponse>();

            //Lesson
            CreateMap<CreateNewLessonForModuleRequest, Lesson>();
            CreateMap<UpdateLessonRequest, Lesson>();
            CreateMap<Lesson, LessonResponse>();
            CreateMap<Lesson, LessonDetailResponse>();

            //LessonResource 
            CreateMap<CreateLessonResourceRequest, LessonResource>();
            CreateMap<LessonResource, LessonResourceResponse>();

            //Module
            CreateMap<CreateNewModuleForCourseRequest, Module>();
            CreateMap<UpdateModuleRequest, Module>();
            CreateMap<Module, ModuleResponse>();

        }
    }
}
