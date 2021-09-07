using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.BL.Models;
using University.BL.DTOs;
using University.BL.Data;
using PagedList;

namespace University.Web.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly UniversityContext context = new UniversityContext();

        [HttpGet]

        public ActionResult Index(int? instructorId, int? pageSize, int? page)
        {

            var query = context.Instructor.ToList();


            var instructors = query.Select(x => new InstructorDTO
            {
                ID = x.ID,
                LastName = x.LastName,
                FirstMidName = x.FirstMidName,
                HireDate = x.HireDate
            }).ToList();

            if (instructorId != null)
            {

                //SELECT r.*
                //FROM[dbo].[Enrollment] q
                //JOIN Course r ON q.CourseID = r.CourseID
                //WHERE q.StudentID = 1

                var department = (from q in context.Instructor
                                  join r in context.Department on q.ID equals r.InstructorID
                                  where q.ID == instructorId
                                  select new DepartmentDTO
                                  {
                                      DepartmentID = r.DepartmentID,
                                      Name = r.Name,
                                      Budget = r.Budget,
                                      StartDate = r.StartDate,
                                      InstructorID = r.InstructorID
                                  }).ToList();

                ViewBag.Courses = department;

            }

            //paginacion
            #region Paginacion
            pageSize = (pageSize ?? 10);
            page = (page ?? 1);
            ViewBag.PageSize = pageSize;
            #endregion


            return View(instructors.ToPagedList(page.Value, pageSize.Value));
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(InstructorDTO instructor)
        {

            if (!ModelState.IsValid)
                return View(instructor);


            context.Instructor.Add(new Instructor
            {
                ID = instructor.ID,
                LastName = instructor.LastName,
                FirstMidName = instructor.FirstMidName,
                HireDate = instructor.HireDate

            });
            context.SaveChanges();


            return RedirectToAction("Index");


        }

       

        [HttpGet]

        public ActionResult Edit(int id)
        {

            var instructor = context.Instructor.Where(x => x.ID == id)
                            .Select(x => new InstructorDTO
                            {
                                ID = x.ID,
                                LastName = x.LastName,
                                FirstMidName = x.FirstMidName,
                                HireDate = x.HireDate
                            }).FirstOrDefault();

            return View(instructor);
        }

        [HttpPost]
        public ActionResult Edit(InstructorDTO instructor)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(instructor);


                //var studentModel = context.Students.Where(x => x.ID == student.ID).Select(x => x).FirstOrDefault();
                var instructorModel = context.Instructor.FirstOrDefault(x => x.ID == instructor.ID);

                //campos que se van a modificar
                //sobreescribo las propiedades del modelo de base de datos
                instructorModel.LastName = instructor.LastName;
                instructorModel.FirstMidName = instructor.FirstMidName;
                instructorModel.HireDate = instructor.HireDate;

                //aplique los cambios en base de datos
                context.SaveChanges();


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(instructor);

        }

        [HttpGet]

        public ActionResult Delete(int id)
        {
            if (!context.Department.Any(x => x.InstructorID == id))
            {
                if (!context.CourseInstructor.Any(x => x.InstructorID == id))
                {
                    if (!context.OfficeAssignments.Any(x => x.InstructorID == id)) 
                    {
                        var instructorModel = context.Instructor.FirstOrDefault(x => x.ID == id);

                        context.Instructor.Remove(instructorModel);

                        context.SaveChanges();
                    }
                    
                }

            }


            return RedirectToAction("Index");
        }
    }
}