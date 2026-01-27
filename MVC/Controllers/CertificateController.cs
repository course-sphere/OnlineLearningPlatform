using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers
{
    public class CertificateController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CertificateController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        public async Task<IActionResult> Index(Guid courseId)
        {
            var course = await _unitOfWork.Courses.GetQueryable().FirstOrDefaultAsync(c => c.CourseId == courseId);
            return View(course);
        }
    }
}