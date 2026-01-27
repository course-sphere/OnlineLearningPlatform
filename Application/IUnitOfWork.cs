using Application.IRepositories;
using Domain.Entities;

namespace Application
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ICourseRepository Courses { get; }
        IEnrollmentRepository Enrollments { get; }
        ILessonRepository Lessons { get; }
        IPaymentRepository Payments { get; }
        IModuleRepository Modules { get; }
        IGradedItemRepository GradedItems { get; }
        IGradedAttemptRepository GradedAttempts { get; }
        IQuestionSubmissionRepository QuestionSubmissions { get; }
        ISubmissionAnswerOptionRepository SubmissionAnswerOptions { get; }
        ILessonResourceRepository LessonResources { get; }

        // Cái cũ của bạn (giữ lại nếu code cũ dùng)
        IUserLessonProgressRepository LessonProgresses { get; }

        // 👇 CÁI MỚI (Dùng cho EnrollmentService)
        IGenericRepository<UserLessonProgress> UserLessonProgress { get; }

        // Các bảng phụ (nếu cần)
        IGenericRepository<Question> Questions { get; }
        IGenericRepository<AnswerOption> AnswerOptions { get; }

        // 👇 Phải trả về int
        Task<int> SaveChangeAsync();
    }
}