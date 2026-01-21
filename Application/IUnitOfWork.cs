using Application.IRepositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public IUserRepository Users { get; }
        public ILessonRepository Lessons { get; }
        public ICourseRepository Courses { get; }
        public IEnrollmentRepository Enrollments { get; }
        public IPaymentRepository Payments { get; }
        Task SaveChangeAsync();
    }
}
