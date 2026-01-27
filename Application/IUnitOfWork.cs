using Application.IRepositories;
using Domain.Entities;

namespace Application
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ILessonRepository Lessons { get; }
        ICourseRepository Courses { get; }
        IEnrollmentRepository Enrollments { get; }
        IPaymentRepository Payments { get; }
        IModuleRepository Modules { get; }
        ILessonResourceRepository LessonResources { get; }
        IUserLessonProgressRepository LessonProgresses { get; }
        IGradedItemRepository GradedItems { get; }
        IGradedAttemptRepository GradedAttempts { get; }
        ISubmissionAnswerOptionRepository SubmissionAnswerOptions { get; }
        IQuestionSubmissionRepository QuestionSubmissions { get; }

        IGenericRepository<AnswerOption> AnswerOptions { get; }
        IGenericRepository<Question> Questions { get; }
        IGenericRepository<UserLessonProgress> UserLessonProgress { get; }

        Task SaveChangeAsync();
    }
}