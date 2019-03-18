using Microsoft.AspNetCore.Mvc;
using School_Models;
using School_Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School_Webapp.ViewComponents
{
    public class NewsViewComponent : ViewComponent
    {
        private readonly INewsRepo newsRepo;

        public NewsViewComponent(INewsRepo newsRepo)
        {
            this.newsRepo = newsRepo;
        }

        //Random News item
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await newsRepo.GetAllNewsAsync(); //logica: random item ophalen 
            int index = new Random().Next(items.Count());
            News randomNews = await Task.FromResult<News>(items.Skip(index).FirstOrDefault());

            return View("Default", randomNews);
        }
    }
}
