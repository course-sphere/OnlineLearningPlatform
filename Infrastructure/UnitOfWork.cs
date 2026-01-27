using Application;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository Users { get; }
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

        // 👇 Property Generic (khớp với EnrollmentService)
        public IGenericRepository<UserLessonProgress> UserLessonProgress { get; private set; }
        public IGenericRepository<Question> Questions { get; private set; }
        public IGenericRepository<AnswerOption> AnswerOptions { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Users = new UserRepository(context);
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

            Questions = new GenericRepository<Question>(context);
            AnswerOptions = new GenericRepository<AnswerOption>(context);

            // 👇 Khởi tạo Generic Repository
            UserLessonProgress = new GenericRepository<UserLessonProgress>(context);
        }

        public async Task<int> SaveChangeAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error here if needed
                throw new Exception("Error while saving changes", ex);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}