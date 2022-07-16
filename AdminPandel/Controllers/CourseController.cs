using AdminPandel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AdminPandel.Controllers
{
    public class CourseController : Controller
    {
        public DashboardDbContext Context { get; }

        public CourseController(DashboardDbContext context)
        {
            Context = context;
        }
        public IActionResult Index(int id)
        {

            var courseCount = Context.Courses.Count();

            ViewData["CoursesCount"] = courseCount;
            var course = Context.Courses
                .OrderBy(x => x.CourseName)
                .ToList();
            return View(course);
        }

        public IActionResult FilterCourses(int skip, int take)
        {
            var course = Context.Courses
               .OrderBy(x => x.CourseName)
               //.OrderByDescending(x => x.CourseName)
               .Skip(skip)
               .Take(take)
               .ToList();
            return View(course);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                var savedC =Context.Courses.Add(course);

                Context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(course);
        }

        [HttpGet]
        public IActionResult Edit(int courseId)
        {
            if(courseId !=0)
            {
                var getCourseById = Context.Courses.FirstOrDefault(x => x.CourseId == courseId);
                return View(getCourseById);
            }
           
            return View("Index");
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                //var getCourseById= Context.Courses.FirstOrDefault(x=> x.CourseId==course.CourseId);

                //getCourseById.CourseName=course.CourseName;

                Context.Courses.Update(course);

                Context.SaveChanges();

                return RedirectToAction("Index");

            }
            return View(course);
        }

        [HttpGet]
        public IActionResult Details(int courseId)
        {
            if (courseId != 0)
            {
                var getCourseById = Context.Courses.FirstOrDefault(x => x.CourseId == courseId);
                var getCourseById2 = Context.Courses.First(x => x.CourseId == courseId);
                return View(getCourseById);
            }
            return RedirectToAction("Index");
        }

       
        public IActionResult Delete(int courseId)
        {
            if (courseId != 0)
            {
                var getCourseById = Context.Courses.SingleOrDefault(x => x.CourseId == courseId);
                //var getCourseById2 = Context.Courses.Single(x => x.CourseId == courseId);

                var checkIfExist = Context.Courses.Any(x => x.CourseId == courseId);
                if (checkIfExist)
                {
                    Context.Courses.Remove(getCourseById);
                    Context.SaveChanges();
                    return RedirectToAction("Index");
                }

             
            }
            return RedirectToAction("Index");
        }


    }
}
