using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext context; 
        public HomeController(MyContext DbContext)
        {
            context = DbContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = context.Dishes.OrderByDescending(d => d.CreatedAt).ToList();
            return View(AllDishes);
        }
        [HttpGet("new")]
        public IActionResult NewDish()
        {
            return View();
        }
        [HttpPost]
        [Route("createdish")]
        public IActionResult CreateDish(Dish dish)
        {
            if(ModelState.IsValid)
            {
                context.Dishes.Add(dish);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("NewDish");
            }
        }
        [HttpGet("{DishId}")]
        public IActionResult ViewDish(int DishId)
        {
            Dish SelectedDish = context.Dishes.SingleOrDefault(d => d.DishId == DishId);
            return View("ViewDish",SelectedDish);
        }
        [HttpGet("edit/{DishId}")]
        public IActionResult EditDish(int DishId)
        {
            Dish EditedDish = context.Dishes.SingleOrDefault(d => d.DishId == DishId);
            return View("EditDish",EditedDish);
        }
        [HttpPost("update")]
        public IActionResult UpdateDish(Dish dish)
        {
            if(ModelState.IsValid)
            {
                context.Update(dish);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("EditDish",dish);
            }
        }
        [HttpGet("delete/{DishId}")]
        public IActionResult DeleteDish(int DishId)
        {
            Dish DeletedDish = context.Dishes.SingleOrDefault(d => d.DishId == DishId);
            context.Dishes.Remove(DeletedDish);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
