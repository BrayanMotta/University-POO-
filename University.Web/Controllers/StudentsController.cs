using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.BL.Models;
using University.BL.DTOs;
using University.BL.Data;


namespace University.Web.Controllers
{
    public class StudentsController : Controller

    {
        private readonly UniversityContext context = new UniversityContext();

        [HttpGet]
        // GET: Students
        public ActionResult Index()
        {

            var students = new List<StudentDTO>(); //ilimitada

            students.Add(new StudentDTO { ID = 1, FirstMidName = "David", LastName = "Santafe", EnrollmentDate = DateTime.Now });
            students.Add(new StudentDTO { ID = 2, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Now });
            students.Add(new StudentDTO { ID = 3, FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Now });

            ViewBag.Data = "Mesaje de prueba";
            ViewBag.Message = "Mesaje de prueba";

           // ViewData["Data"] = "Mensaje de prueba";

            return View(students);
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentDTO student)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(student);

                if (student.EnrollmentDate > DateTime.Now)
                    throw new Exception("La fecha de matricula no puede ser mayor a la fecha actual");

                //INSERT INTO Students(FirstMidName,LastName,EnrollmentDate) VALUES(@FirstMidName, @LastName, @EnrollmentDate)
                context.Students.Add(new Student { 
                    FirstMidName = student.FirstMidName,
                    LastName = student.LastName,
                    EnrollmentDate = student.EnrollmentDate
                
                });
                context.SaveChanges();
                

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(student);
  
        }
    }
}