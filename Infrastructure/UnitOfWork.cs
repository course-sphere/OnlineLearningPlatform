using Application;
using Application.IRepositories;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;

        public IUserRepository Users { get; set; }
        public IAnswerOptionRepository AnswerOptions { get; set; }
        public IAssignmentRepository Assignments { get; set; }
        public IQuestionRepository Questions { get; set; }
        public IQuizRepository Quizzes { get; set; }
        public ICourseRepository Courses { get; set; }
        public IEnrollmentRepository Enrollments { get; set; }
        public ILessonRepository Lessons { get; set; }
        public IPaymentRepository Payments { get; set; }
        public IQuizAttemptRepository QuizAttempts { get; set; }
        public ISubmissionRepository Submissions { get; set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            AnswerOptions = new AnswerOptionRepository(context);
            Assignments = new AssignmentRepository(context);
            Questions = new QuestionRepository(context);
            Quizzes = new QuizRepository(context);
            Courses = new CourseRepository(context);
            Enrollments = new EnrollmentRepository(context);
            Lessons = new LessonRepository(context);
            Payments = new PaymentRepository(context);
            QuizAttempts = new QuizAttemptRepository(context);
            Submissions = new SubmissionRepository(context);
        }

        public async Task SaveChangeAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(message: ex.Message);
            }
        }
    }
}
