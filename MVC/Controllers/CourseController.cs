using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        public IActionResult Learn(string id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        public IActionResult Read(string id)
        {
            ViewBag.LessonId = id;
            return View();
        }

        [Route("Courses/QuizResult/Pass")]
        public IActionResult QuizResultPass()
        {
            return View();
        }

        [Route("Courses/QuizResult/Fail")]
        public IActionResult QuizResultFail()
        {
            return View();
        }
    }
}
