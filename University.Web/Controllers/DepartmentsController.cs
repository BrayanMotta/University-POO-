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
    public class DepartmentsController : Controller
    {
        private readonly UniversityContext context = new UniversityContext();

        [HttpGet]

        public ActionResult Index(int? pageSize, int? page)
        {

            var query = context.Department.ToList();


            var deparment = query.Select(x => new DepartmentDTO
            {
                DepartmentID = x.DepartmentID,
                Name = x.Name,
                Budget = x.Budget,
                StartDate = x.StartDate,
                InstructorID = x.InstructorID
            }).ToList();


            //paginacion
            #region Paginacion
            pageSize = (pageSize ?? 10);
            page = (page ?? 1);
            ViewBag.PageSize = pageSize;
            #endregion


            return View(deparment.ToPagedList(page.Value, pageSize.Value));
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DepartmentDTO department)
        {

            if (!ModelState.IsValid)
                return View(department);


            context.Department.Add(new Department
            {
                DepartmentID = department.DepartmentID,
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                InstructorID = department.InstructorID

            });
            context.SaveChanges();


            return RedirectToAction("Index");


        }



        [HttpGet]

        public ActionResult Edit(int id)
        {

            var department = context.Department.Where(x => x.DepartmentID == id)
                            .Select(x => new DepartmentDTO
                            {
                                DepartmentID = x.DepartmentID,
                                Name = x.Name,
                                Budget = x.Budget,
                                StartDate = x.StartDate,
                                InstructorID = x.InstructorID
                            }).FirstOrDefault();

            return View(department);
        }

        [HttpPost]
        public ActionResult Edit(DepartmentDTO department)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(department);


                //var studentModel = context.Students.Where(x => x.ID == student.ID).Select(x => x).FirstOrDefault();
                var departmentModel = context.Department.FirstOrDefault(x => x.DepartmentID == department.DepartmentID);

                //campos que se van a modificar
                //sobreescribo las propiedades del modelo de base de datos
                departmentModel.Name = departmentModel.Name;
                departmentModel.Budget = departmentModel.Budget;
                departmentModel.StartDate = departmentModel.StartDate;
                departmentModel.InstructorID = departmentModel.InstructorID;

                //aplique los cambios en base de datos
                context.SaveChanges();


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(department);

        }

        [HttpGet]

        public ActionResult Delete(int id)
        {

            var departmentModel = context.Department.FirstOrDefault(x => x.DepartmentID == id);

            context.Department.Remove(departmentModel);

            context.SaveChanges();



            return RedirectToAction("Index");
        }
    }
}