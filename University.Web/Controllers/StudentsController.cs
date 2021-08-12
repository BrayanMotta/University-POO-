using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Web.Models;
using System.Collections.Generic;

namespace University.Web.Controllers
{
    public class StudentsController : Controller
    {
        [HttpGet]
        // GET: Students
        public ActionResult Index()
        {

            var students = new List<Student>(); //ilimitada

            students.Add(new Student { ID = 1, FirstMidName = "David", LastName = "Santafe", EnrollmentDate = DateTime.Now });
            students.Add(new Student { ID = 2, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Now });
            students.Add(new Student { ID = 3, FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Now });

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
        public ActionResult Create(Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(student);

                if (student.EnrollmentDate > DateTime.Now)
                    throw new Exception("La fecha de matricula no puede ser mayor a la fecha actual");

                var id = student.ID;
                var FirstMidName = student.FirstMidName;
                var LastName = student.LastName;
                var enrollmentDate = student.EnrollmentDate;
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(student);
  
        }
    }
}