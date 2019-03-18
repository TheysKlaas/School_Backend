using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School_Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using School_Webapp.ViewModels;
using Microsoft.Extensions.Logging;

namespace School_Models.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IEducationRepo educationRepo;
        private readonly ILogger<StudentController> logger;

        public StudentController(IStudentRepo studentRepo, IEducationRepo educationRepo, ILogger<StudentController> logger)
        {
            this.studentRepo = studentRepo;
            this.educationRepo = educationRepo;
            this.logger = logger;
        }

        // GET: Student ---------------------------------------------------READ 
        public async Task<ActionResult> IndexAsync(string search = null)
        {
            var students = await studentRepo.GetAllStudentsAsync(search);
            if (!String.IsNullOrEmpty(search))
            {
                students = await studentRepo.GetAllStudentsAsync(search);
            }
            return View(students);
        }

        //GET: Search and Order Form
        public ActionResult Frm_search_order() => View();


        public async Task<ActionResult> Details(int? id)
        {
            ////indien een nullable int? id
            if (id == null)
            {
                return BadRequest();
            }
            var stu = await studentRepo.GetStudentAsync(id.Value);
            if (stu == null)
            {
                //Of: Zorgt voor taalafhankelijk browser error:
                //return NotFound(); //404
                //return BadRequest(); //400
                ////Of: Zorgt voor customised error volgens jouw Viewmodel:
                var errorvm = new ErrorViewModel();

                errorvm.RequestId = Convert.ToString(id.Value);
                errorvm.HttpStatuscode = System.Net.HttpStatusCode.NotFound;
                return View("~/Views/Shared/_Error.cshtml", errorvm);
            }
            return View(stu);
        }


        // GET: Student/Create
        public async Task<ActionResult> Create()
        {
            List<Education> educationList = new List<Education>();
            educationList.Insert(0, new Education { Name = "--Kies een opleiding --" });
            educationList.AddRange(await educationRepo.GetAllEducationsAsync());

            SelectList selectListItems = new SelectList(educationList, "Id", "Name");
            ViewData["Educations"] = selectListItems;
            return View();
        }

        //public ActionResult Frm_StudentsForEducation()
        //{
        //    return View();
        //}

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection formcollection,[Bind("Birthday,Email,Name,Gender, Password,EducationId")] Student student)
        {

            if (ModelState.IsValid)
            {
                try
                { 
                    // TODO: Add insert logic here 

                    Student result = await studentRepo.Create(student);
                    logger.LogWarning(LogEventIds.InsertLogId, "student {name} toegevoegd met ID {Id}", result.Name, result.Id);
                    logger.LogInformation(LogEventIds.InsertLogId, "Student {Name }toegevoegd met ID: {ID}", student.Name, student.Id);
                    if(result == null)
                        throw new Exception();
                    return RedirectToAction(nameof(IndexAsync));
                }
                catch(Exception ex)
                {
                    logger.Log(LogLevel.Information, 0, typeof(Person), null, (type, exception) => "Hello from logger");
                    logger.LogInformation("User {userName} entered at {entryTime}", student.Name, DateTime.Now);
                    logger.LogWarning(LogEventIds.InsertLogId, "er was geen {MODEL} aanwezig", student);
                    ModelState.AddModelError("", "Create is unable to save."+ ex);
                    return View();
                }
            }
            else
            {
                return View(student);
            }
        }

        // GET: Education/Edit/5 ---------------------------------------------EDIT 
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var stu = await studentRepo.GetStudentAsync(id.Value);
            if (stu == null)
            {
                return NotFound();
            }

            StudentEducationVm vm = new StudentEducationVm(educationRepo, stu);
            return View(vm);
        }


        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection formcollection, StudentEducationVm vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await studentRepo.Update(vm.Student);
                }
                catch (Exception ex)
                {
                    if (await studentRepo.StudentExists(vm.Student.Id) == false)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexAsync));
            }
            return View(vm);
        }

        // GET: Student/Delete/5 ----------------------------------------DELETE 
        public async Task<ActionResult> Delete(int id)
        {
            var stu = await studentRepo.GetStudentAsync(id);
            if (stu == null)
                return NotFound();
            return View(stu);
        }
        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var stu = await studentRepo.GetStudentAsync(id);
                if (stu == null)
                    return NotFound();
                await studentRepo.Delete(stu);
                return RedirectToAction(nameof(IndexAsync));
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                var errorvm = new ErrorViewModel();
                errorvm.RequestId = Convert.ToString(id);
                errorvm.HttpStatuscode = System.Net.HttpStatusCode.BadRequest;
                return View("~/Views/Shared/_Error.cshtml", errorvm);

            }
        }
    }
}