using AutoMapper;
using Domain.Entities;
using Domain.Requests.Course;
using Domain.Requests.Enrollment;
using Domain.Requests.Lesson;
using Domain.Requests.LessonResource;
using Domain.Requests.Module;
using Domain.Requests.Question;      // <--- QUAN TRỌNG: Thêm using này
using Domain.Requests.AnswerOption;  // <--- QUAN TRỌNG: Thêm using này
using Domain.Responses.Course;
using Domain.Responses.Lesson;
using Domain.Responses.LessonResource;
using Domain.Responses.Module;
using Domain.Responses.GradedItem;   // <--- QUAN TRỌNG: Thêm using này

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

            CreateMap<CreateNewModuleForCourseRequest, Module>();
            CreateMap<UpdateModuleRequest, Module>();
            CreateMap<Module, ModuleResponse>();

            CreateMap<CreateNewLessonForModuleRequest, Lesson>();
            CreateMap<UpdateLessonRequest, Lesson>();

            CreateMap<Lesson, LessonResponse>();

            CreateMap<Lesson, LessonDetailResponse>();

            CreateMap<CreateLessonResourceRequest, LessonResource>();
            CreateMap<LessonResource, LessonResourceResponse>();

            CreateMap<GradedItem, GradedItemResponse>();

            CreateMap<CreateQuestionRequest, Question>()
                .ForMember(dest => dest.AnswerOptions, opt => opt.Ignore());
            CreateMap<CreateAnswerOptionRequest, AnswerOption>();
        }
    }
}