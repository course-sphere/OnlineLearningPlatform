using AutoMapper;
using Domain.Entities;
// Requests
using Domain.Requests.Course;
using Domain.Requests.Enrollment;
using Domain.Requests.Lesson;
using Domain.Requests.LessonResource;
using Domain.Requests.Module;
using Domain.Requests.Question;
using Domain.Requests.AnswerOption;
// Responses
using Domain.Responses.Course;
using Domain.Responses.Lesson;
using Domain.Responses.LessonResource;
using Domain.Responses.Module;
using Domain.Responses.GradedItem;

namespace Application.MyMapper
{
    public class MapperConfigurationProfile : Profile
    {
        public MapperConfigurationProfile()
        {
            // Enrollment
            CreateMap<CreateNewEnrollementRequest, Enrollment>();

            // Course
            CreateMap<CreateNewCourseRequest, Course>();
            CreateMap<Course, CourseResponse>();
            CreateMap<UpdateCourseRequest, Course>();
            CreateMap<Course, GetAllCourseForAdminResponse>();

            // Module
            CreateMap<CreateNewModuleForCourseRequest, Module>();
            CreateMap<UpdateModuleRequest, Module>();

            CreateMap<Module, Domain.Responses.Module.ModuleResponse>();

            // Lesson
            CreateMap<CreateNewLessonForModuleRequest, Lesson>();
            CreateMap<UpdateLessonRequest, Lesson>();

            CreateMap<Lesson, Domain.Responses.Lesson.LessonResponse>();

            CreateMap<Lesson, LessonDetailResponse>();

            // Lesson Resource
            CreateMap<CreateLessonResourceRequest, LessonResource>();
            CreateMap<LessonResource, LessonResourceResponse>();

            // Graded Item
            CreateMap<GradedItem, GradedItemResponse>();

            // Question & Answer
            CreateMap<CreateQuestionRequest, Question>()
                .ForMember(dest => dest.AnswerOptions, opt => opt.Ignore());
            CreateMap<CreateAnswerOptionRequest, AnswerOption>();
        }
    }
}