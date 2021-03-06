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
    public class CoursesController : Controller
    {
        private readonly UniversityContext context = new UniversityContext();

        [HttpGet]
 
        public ActionResult Index(int? courseId, int? pageSize, int? page)
        {

            var query = context.Courses.ToList();

            
            var courses = query.Select(x => new CourseDTO
                            {
                                CourseID = x.CourseID,
                                Title = x.Title,
                                Credits = x.Credits,
                            }).ToList();

            if (courseId != null)
            {

                //SELECT r.*
                //FROM[dbo].[Enrollment] q
                //JOIN Course r ON q.CourseID = r.CourseID
                //WHERE q.StudentID = 1

                var instructor = (from q in context.CourseInstructor
                               join r in context.Instructor on q.InstructorID equals r.ID
                               where q.CourseID == courseId
                               select new InstructorDTO
                               {
                                   ID = r.ID,
                                   LastName = r.LastName,
                                   FirstMidName = r.FirstMidName
                               }).ToList();

                ViewBag.Courses = instructor;

            }

            //paginacion
            #region Paginacion
            pageSize = (pageSize ?? 10);
            page = (page ?? 1);
            ViewBag.PageSize = pageSize;
            #endregion


            return View(courses.ToPagedList(page.Value, pageSize.Value));
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CourseDTO course)
        {
            
                if (!ModelState.IsValid)
                    return View(course);

                
                context.Courses.Add(new Course
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Credits = course.Credits

                });
                context.SaveChanges();


                return RedirectToAction("Index");


        }

        private void LoadData()
        {
            var courses = context.Courses.Select(x => new CourseDTO
            {
                CourseID = x.CourseID,
                Title = x.Title,
                Credits = x.Credits

            }).ToList();

            ViewData["Courses"] = new SelectList(courses, "CourseID", "Title");
        }

        [HttpGet]

        public ActionResult Edit(int id)
        {
            
            var course = context.Courses.Where(x => x.CourseID == id)
                            .Select(x => new CourseDTO
                            {
                                CourseID = x.CourseID,
                                Title = x.Title,
                                Credits = x.Credits
                            }).FirstOrDefault();

            return View(course);
        }

        [HttpPost]
        public ActionResult Edit(CourseDTO course)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(course);

                
                //var studentModel = context.Students.Where(x => x.ID == student.ID).Select(x => x).FirstOrDefault();
                var courseModel = context.Courses.FirstOrDefault(x => x.CourseID == course.CourseID);

                //campos que se van a modificar
                //sobreescribo las propiedades del modelo de base de datos
                courseModel.Title = course.Title;
                courseModel.Credits = course.Credits;

                //aplique los cambios en base de datos
                context.SaveChanges();


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(course);

        }

        [HttpGet]

        public ActionResult Delete(int id)
        {
            if (!context.Enrollments.Any(x => x.CourseID == id))
            {
                if (!context.CourseInstructor.Any(x => x.CourseID == id)) 
                {
                    var courseModel = context.Courses.FirstOrDefault(x => x.CourseID == id);

                    context.Courses.Remove(courseModel);

                    context.SaveChanges();
                }
                   
            }


            return RedirectToAction("Index");
        }
    }
}