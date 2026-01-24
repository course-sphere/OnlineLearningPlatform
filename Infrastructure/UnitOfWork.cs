using Application;
using Application.IRepositories;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository Users { get; }
        public IAnswerOptionRepository AnswerOptions { get; }
        public IQuestionRepository Questions { get; }
        public ICourseRepository Courses { get; }
        public IEnrollmentRepository Enrollments { get; }
        public ILessonRepository Lessons { get; }
        public IPaymentRepository Payments { get; }
        public IModuleRepository Modules { get; }
        public IGradedItemRepository GradedItems { get; }
        public IGradedAttemptRepository GradedAttempts { get; }
        public IQuestionSubmissionRepository QuestionSubmissions { get; }
        public ISubmissionAnswerOptionRepository SubmissionAnswerOptions { get; }
        public ILessonResourceRepository LessonResources { get; }
        public IUserLessonProgressRepository LessonProgresses { get; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Users = new UserRepository(context);
            AnswerOptions = new AnswerOptionRepository(context);
            Questions = new QuestionRepository(context);
            Courses = new CourseRepository(context);
            Enrollments = new EnrollmentRepository(context);
            Lessons = new LessonRepository(context);
            Payments = new PaymentRepository(context);
            GradedItems = new GradedItemRepository(context);
            GradedAttempts = new GradedAttemptRepository(context);
            QuestionSubmissions = new QuestionSubmissionRepository(context);
            SubmissionAnswerOptions = new SubmissionAnswerOptionRepository(context);
            Modules = new ModuleRepository(context);
            LessonResources = new LessonResourceRepository(context);
            LessonProgresses = new UserLessonProgressRepository(context);
        }

        public async Task SaveChangeAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while saving changes", ex);
            }
        }
    }
}
