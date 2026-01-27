using Application;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        // 1. Khai báo Property (Mỗi cái chỉ khai báo 1 lần)
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}