using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {

        [HttpGet("/categories")]
        public ActionResult Index()
        {
            List<Category> allCategories = Category.GetAll();
            return View(allCategories);
        }

        [HttpGet("/categories/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/categories")]
        public ActionResult Create(string categoryName)
        {
            Category newCategory = new Category(categoryName);
            newCategory.Save();
            return RedirectToAction("Index");
        }

        [HttpGet("/categories/{id}/{sortBy}")]
        public ActionResult SortByDueDate(int id, string sortBy)
        {
            return RedirectToAction("Show", new { id = id, sortBy });
        }

        [HttpGet("/categories/{id}")]
        public ActionResult Show(int id, string sortBy = "")
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(id);
            List<Item> categoryItems = selectedCategory.GetItems(sortBy);
            model.Add("category", selectedCategory);
            // Console.WriteLine("category {0} {1}", selectedCategory.GetId(), selectedCategory.GetName());
            model.Add("items", categoryItems);
            // Console.WriteLine("items {0}", categoryItems.Count);
            return View(model);
        }

        [HttpPost("/categories/{categoryId}/items")]
        public ActionResult Create(string itemDescription, DateTime dueDate, int categoryId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category foundCategory = Category.Find(categoryId);
            Item newItem = new Item(itemDescription, dueDate, categoryId);
            newItem.Save();
            foundCategory.AddItem(newItem);
            List<Item> categoryItems = foundCategory.GetItems();
            model.Add("items", categoryItems);
            model.Add("category", foundCategory);
            return View("Show", model);
        }

        [HttpPost("/categories/{categoryId}")]
        public ActionResult DeleteItem(int categoryId, int itemId)
        {
            Item item = Item.Find(itemId);
            item.Delete();
            return RedirectToAction("Index");
        }
    }
}
