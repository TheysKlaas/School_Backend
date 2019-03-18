using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_Models;
using School_Models.Repositories;

namespace School_Webapp.Controllers
{
    public class EducationController : Controller
    {
        private readonly IEducationRepo educationRepo;

        public EducationController(IEducationRepo educationRepo)
        {
            this.educationRepo = educationRepo;
        }
        // GET: Education ---------------------------------------------------READ 
        public async Task<ActionResult> IndexAsync(string search = null)
        {
            var educations = await educationRepo.GetAllEducationsAsync(search);
            if (!String.IsNullOrEmpty(search))
            {
                educations =
                await educationRepo.GetAllEducationsAsync(search);
            }
            return View(educations); ;
        }
        //GET: Search and Order Form
        public ActionResult Frm_search_order() => View();
        // GET: Education/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ////indien een nullable int? id
            if (id == null)
            {
                return BadRequest();
            }
            var edu = await educationRepo.GetEducationAsync(id.Value);
            if (edu == null)
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
            return View(edu);
        }
        // GET: Education/Create --------------------------------------------CREATE 
        public ActionResult Create()
        {
            return View();
        }
        // POST: Education/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
        [Bind("Name,Code,Description")] IFormCollection collection,
       [Bind("Name,Code,Description")] Education education)
        {
            if (ModelState.IsValid)
            {
                //1. Valid
                try
                {
                    //Add insert logic here
                    Education result = await educationRepo.Create(education);
                    if (result == null)
                        throw new Exception();
                    return RedirectToAction(nameof(IndexAsync));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError("", "Create is unable to save.");
                    return View(education);
                }
            }
            else
            {
                //2. Invalid >> return
                return View(education);
            }
        }
        // GET: Education/Edit/5 ---------------------------------------------EDIT 
        public async Task<ActionResult> Edit(int id)
        {
            ////indien nullable id:
            //if (id == null)
            //{
            // return NotFound();
            //}
            var edu = await educationRepo.GetEducationAsync(id);
            if (edu == null)
            {
                return NotFound();
            }
            return View(edu);
        }
        // POST: Education/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,
        [Bind("Id,Name,Code,Description")] IFormCollection collection,
        [Bind("Id,Name,Code,Description")] Education education)
        {
            if (id != education.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await educationRepo.Update(education);
                }
                catch (Exception)
                {
                    if (await educationRepo.EducationExists(education.Id) == false)
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
            return View(education);
        }
        // GET: Education/Delete/5 ----------------------------------------DELETE 
        public async Task<ActionResult> Delete(int id)
        {
            var edu = await educationRepo.GetEducationAsync(id);
            if (edu == null)
                return NotFound();
            return View(edu);
        }
        // POST: Education/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // Add delete logic here
                // if (id == null) return NotFound(); //indien nullable id
                var edu = await educationRepo.GetEducationAsync(id);
                if (edu == null)
                    return NotFound();
                await educationRepo.Delete(edu);
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
