using Application.IRepositories;
using Domain.Entities;

namespace Application
{
    public interface IUnitOfWork
    {
        public IUserRepository Users { get; }
        public ILessonRepository Lessons { get; }
        public ICourseRepository Courses { get; }
        public IEnrollmentRepository Enrollments { get; }
        public IPaymentRepository Payments { get; }
        public IModuleRepository Modules { get; }
        public ILessonResourceRepository LessonResources { get; }
        public IUserLessonProgressRepository LessonProgresses { get; }
        public IGradedItemRepository GradedItems { get; }
        public IGradedAttemptRepository GradedAttempts { get; }
        public ISubmissionAnswerOptionRepository SubmissionAnswerOptions { get; }
        public IQuestionSubmissionRepository QuestionSubmissions { get; }
        public IAnswerOptionRepository AnswerOptions { get; }
        Task SaveChangeAsync();
    }
}
