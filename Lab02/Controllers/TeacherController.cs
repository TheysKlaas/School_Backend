using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School_Models;
using School_Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_Webapp.ViewModels;

namespace School_Models.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherRepo teacherRepo;
        private readonly IEducationRepo educationRepo;

        public TeacherController(ITeacherRepo teacherRepo, IEducationRepo educationRepo)
        {
            this.teacherRepo = teacherRepo;
            this.educationRepo = educationRepo;
        }

        // GET: Teacher
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Teacher> model = await teacherRepo.GetAllAsync();
            return View(model);
            
            
        }

        // GET: Teacher/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Teacher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: Teacher/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teacher = await teacherRepo.GetTeacherForId(id.Value);
            if (teacher == null)
            {
                return NotFound();
            }
            TeacherEducationsVM vm = new TeacherEducationsVM(educationRepo, teacher);
            return View(vm);
        }

        // POST: Teacher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection, TeacherEducationsVM vm)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    return View(new TeacherEducationsVM(educationRepo, vm.Teacher));
                }
                await teacherRepo.Update(vm.Teacher);

                teacherRepo.RemoveEducation(vm.Teacher.Id);

                teacherRepo.AddEducationsToTeacher(vm.Teacher.Id, vm.SelectedEducationsString);

                
                return RedirectToAction(nameof(IndexAsync));
            }
            catch (Exception ex)
            {
                return View(new TeacherEducationsVM(educationRepo, vm.Teacher));
            }
        }

        // GET: Teacher/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Teacher/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}