using AdminPandel.Models;
using AdminPandel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AdminPandel.Controllers
{
    [Authorize]
    public class ProfesorController : Controller
    {

        public DashboardDbContext Context { get; }
        public IWebHostEnvironment Env { get; }

        public ProfesorController(DashboardDbContext context, IWebHostEnvironment env)
        {
            Context = context;
            Env = env;
        }
        public IActionResult Index()
        {
            return View(Context.Profesors.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {

            var courses = Context.Courses.ToList();

            ViewData["Courses"] = new SelectList(courses, "CourseId", "CourseName");

            return View();
        }
        [HttpPost]
        public IActionResult Create(ProfesorViewModel profVm)
        {
            if (ModelState.IsValid)
            {

                //IFormFile img = profVm.Image;
                //var imgreq = this.Request.Form.Files;

                if (profVm.Images.Count > 2)
                {
                    ModelState.AddModelError("Images", "Vetem dy foto jane te lejuara per ngarkim");
                    var courses = Context.Courses.ToList();
                    ViewData["Courses"] = new SelectList(courses, "CourseId", "CourseName");
                    return View(profVm);
                }

                // Krijimi i direktoriumit per ruajtjen e file-s
                //string crootFilePath = Env.ContentRootPath;
                string rootFilePath = Env.WebRootPath;
                string filePath = Path.Combine(rootFilePath, "Document");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string fileName = $"{Guid.NewGuid().ToString()}_{profVm.Image.FileName}";

                string fullPath = Path.Combine(filePath, fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    profVm.Image.CopyTo(fileStream);
                }

                List<PersonImg> PersonImgs = new();

                foreach (var imgf in profVm.Images)
                {
                    string fileNamef = $"{Guid.NewGuid().ToString()}_{imgf.FileName}";
                    string fullPathf = Path.Combine(filePath, fileNamef);
                    using (var fileStream = new FileStream(fullPathf, FileMode.Create))
                    {
                        profVm.Image.CopyTo(fileStream);
                    }
                    PersonImgs.Add(new PersonImg() { Path = fileNamef });

                }

                var prof = new Profesor()
                {
                    FirstName = profVm.FirstName,
                    LastName = profVm.LastName,
                    PagaNeto = profVm.PagaNeto,
                    CourseId = profVm.CourseId,
                    ImgFilePath = fileName,
                    PersonImgs = PersonImgs,

                };

                Context.Profesors.Add(prof);
                Context.SaveChanges();
                return RedirectToAction("Index", Context.Profesors.ToList());
            }

            return View(profVm);
        }

        public IActionResult Details(int id)
        {
            if (id != null || id != 0)
            {
                var getProfesorById = Context.Profesors.Where(x => x.Id == id).FirstOrDefault();

                ViewData["Kursi"] = Context.Courses.Where(x => x.CourseId == getProfesorById.CourseId).FirstOrDefault().CourseName;

                return View(getProfesorById);

            }

            var Profesors = Context.Profesors.ToList();

            return RedirectToAction("Index", Profesors);
        }


        [HttpGet()]
        public IActionResult Edit(int id)
        {
            if (id != null || id != 0)
            {

                var getProfesorById = Context.Profesors.Where(x => x.Id == id).FirstOrDefault();

                var courses = Context.Courses.ToList();

                ViewData["Courses"] = new SelectList(courses, "CourseId", "CourseName");

                return View(getProfesorById);

            }
            var profesors = Context.Profesors.ToList();

            return RedirectToAction("Index", profesors);

        }

        [HttpPost()]
        public IActionResult Edit(Profesor editProf)
        {
            if (ModelState.IsValid)
            {
                var getProfById = Context.Profesors.Where(x => x.Id == editProf.Id).FirstOrDefault();

                getProfById.FirstName = editProf.FirstName;
                getProfById.LastName = editProf.LastName;
                getProfById.PagaNeto = editProf.PagaNeto;
                getProfById.CourseId = editProf.CourseId;

                Context.SaveChanges();

                var profesors = Context.Profesors.ToList();
                return View("Index", profesors);

            }

            return View(editProf);

        }



        public IActionResult Delete(int id)
        {

            if (id != null || id != 0)
            {

                var result = Context.Profesors.Where(x => x.Id == id).FirstOrDefault();

                Context.Profesors.Remove(result);
                Context.SaveChanges();

                var profesors = Context.Profesors.ToList();

                return RedirectToAction("Index", profesors);

            }

            var prof = Context.Profesors.ToList();

            return RedirectToAction("Index", prof);
        }

        #region Koncepte shtese


        public Profesor IQueryableExample(int id)
        {
            // IQueryable vs IEnumerable
            IQueryable<Profesor> query = Context.Profesors.Where(x => x.PagaNeto > 100);
            if (id == 6)
                query = Context.Profesors.Where(x => x.CourseId == 3);
            else
                query = Context.Profesors.Where(x => x.CourseId == 1);

            var resultIQ = query.FirstOrDefault();
            return resultIQ;
        }

        public Profesor JsonDeserializeExample()
        {
            // Get Data from Json File

            // Convert JSON to string
            var jsonData = System.IO.File.ReadAllText("profesori.json");
            // Deserialize JSON string to object
            var profesor = JsonSerializer.Deserialize<Profesor>(jsonData);

            return profesor;
        }

        public List<int> LinqExampe()
        {
            List<int> list = new List<int>();

            var getNo = GetNo()
                .Where((x) =>
                {
                    return x % 2 == 0;
                })
                .Select(n =>
                {
                    return n * 3;
                });

            foreach (var no in getNo)
            {
                list.Add(no);
            }


            return list;

        }

        IEnumerable<int> GetNo()
        {
            for (int i = 0; i <= 10; i++)
            {
                yield return i;

            }

        }
        #endregion

    }
}
