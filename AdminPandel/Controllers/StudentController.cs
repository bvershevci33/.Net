using AdminPandel.Models;
using AdminPandel.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPandel.Controllers
{
    public class StudentController : Controller
    {
        public StudentController(DashboardDbContext context)
        {
            Context = context;
        }

        public DashboardDbContext Context { get; }

        public IActionResult Index()
        {
            var students = Context.Students
                .Include(x => x.Profesor).AsNoTracking()
                .ToList();

            var result = Context.Database.ExecuteSqlRaw("Select s.*, p.Emri from Students s inner join profesors p on p.id= s.profesorId");
            var studentRaw = Context.Students.FromSqlRaw(" Select *  from Students where id={0}", 7).ToList();
            var studentRaw1 = Context.Students.FromSqlInterpolated($" Select *  from Students where id={7}").ToList();


            var param = new SqlParameter("@FirstName", "Maxi");


            var studentsSp = Context.Students.FromSqlRaw("GetStudents @FirstName", param).ToList();

            var resultSp = Context.Database.ExecuteSqlRaw("Exec GetStudents @FirstName", param);


            return View(students);
        }

        public IActionResult GetProfStudentName()
        {

            var students = Context.Students
              .Include(nameof(Profesor)) // "Profesor"
              .Select(x => new ProfStudentName() { ProfesorName = x.Profesor.FirstName, StudentName = x.FirstName })
              .ToList();

            return View("SutendtsProf", students);

        }

        [HttpGet]
        public IActionResult Create()
        {
            var profersors = Context.Profesors.ToList();
            //var courses = Context.Courses.ToList();

            //TempData["Profesors"] = new SelectList(profersors, "Id", "FirstName");
            //TempData["Courses"] = new SelectList(courses, "CourseId", "CourseName");

            var student = new StudentVm();

            //student.Courses = new SelectList(courses, "CourseId", "CourseName");
            student.Profesors = new SelectList(profersors, "Id", "FirstName");


            return View(student);
        }

        [HttpPost]
        public IActionResult Create(StudentVm std, IFormFile photo)
        {

            //var files = this.HttpContext.Request.Form.Files[0];
            StudentVm studentVm = null;
            List<Profesor> profesors = null;

            if (ModelState.IsValid)
            {
                if (std.FirstName.Substring(0, 1) == "B")
                {
                    ModelState.AddModelError("FirstName", "Emri i dhene nuk eshte ne rregull.");

                    studentVm = new StudentVm();
                    profesors = Context.Profesors.ToList();

                    std.Profesors = new SelectList(profesors, "Id", "FirstName");
                    return View(std);
                }

                byte[] byteImg = null;
                using (var stream = photo.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        byteImg = memoryStream.ToArray();
                    }
                }




                // Course course = new Course() { CourseName = ".Net 6" };

                // std.Profesor = new Profesor() { FirstName = "Dritan", LastName = "Krasniqi", PagaNeto = 234, CourseId=3};

                //var getProf = Context.Profesors.FirstOrDefault(x=> x.Id == 5);

                Student student = new Student();

                student.FirstName = std.FirstName;
                student.LastName = std.LastName;
                student.Address = std.Address;
                student.Age = std.Age;
                student.StatusiStudentit = std.StatusiStudentit;
                student.ProfesorId = std.ProfesorId;
                student.Photo = byteImg;

                //var profersor = Context.Profesors.FirstOrDefault(x => x.Id == std.ProfesorId);
                //student.Profesor= profersor ;

                //student.Profesor.CourseId = std.CourseId;


                Context.Students.Add(student);

                Context.SaveChanges();

                var stutdents = Context.Students.ToList();
                return View("Index", stutdents);

            }

            studentVm = new StudentVm();
            profesors = Context.Profesors.ToList();

            studentVm.Profesors = new SelectList(profesors, "Id", "FirstName");
            return View(studentVm);
        }

        public IActionResult Details(int id)
        {
            if (id != null || id != 0)
            {
                var getStudentById = Context.Students.Where(x => x.Id == id).FirstOrDefault();

                string base64Format = "data:image/jpg;base64";
                string base64ImgConverted = Convert.ToBase64String(getStudentById.Photo);

                string finalBase64Img = $"{base64Format}, {base64ImgConverted}";

                var studentVm = new StudentVm();

                studentVm.FirstName = getStudentById.FirstName;
                studentVm.LastName = getStudentById.LastName;
                studentVm.Age = getStudentById.Age;
                studentVm.StatusiStudentit = getStudentById.StatusiStudentit;
                studentVm.Basae64Img = finalBase64Img;



                return View(studentVm);

            }

            var stutdents = Context.Students.ToList();

            return RedirectToAction("Index", stutdents);
        }


        [HttpGet()]
        public IActionResult Edit(int id)
        {
            if (id != null || id != 0)
            {

                var getStudentById = Context.Students.Where(x => x.Id == id).FirstOrDefault();
                return View(getStudentById);

            }
            var stutdents = Context.Students.ToList();

            return RedirectToAction("Index", stutdents);

        }

        [HttpPost()]
        public IActionResult Edit(Student editStd)
        {
            if (ModelState.IsValid)
            {
                var getStudentById = Context.Students.Where(x => x.Id == editStd.Id).FirstOrDefault();

                getStudentById.FirstName = editStd.FirstName;
                getStudentById.LastName = editStd.LastName;
                getStudentById.Address = editStd.Address;
                getStudentById.Age = editStd.Age;
                getStudentById.StatusiStudentit = editStd.StatusiStudentit;

                Context.SaveChanges();

                var stutdents = Context.Students.ToList();
                return View("Index", stutdents);

            }

            return View(editStd);

        }



        public IActionResult Delete(int id)
        {

            if (id != null || id != 0)
            {

                var getStudentById = Context.Students.Where(x => x.Id == id).FirstOrDefault();

                Context.Students.Remove(getStudentById);
                Context.SaveChanges();

                var stutdents = Context.Students.ToList();

                return View("Index", stutdents);

            }

            var stds = Context.Students.ToList();

            return RedirectToAction("Index", stds);
        }
    }
}

