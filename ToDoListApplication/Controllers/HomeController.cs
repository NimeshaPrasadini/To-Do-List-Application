using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDoListApplication.Models;

namespace ToDoListApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoListDbContext toDoListDbContext;

        public HomeController(ToDoListDbContext context) => toDoListDbContext = context;

        public IActionResult Index(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;

            ViewBag.Priorities = toDoListDbContext.Priorities.ToList();
            ViewBag.Statuses = toDoListDbContext.Statuses.ToList();
            ViewBag.DueFilters = Filters.DueFilterValues;

            IQueryable<ToDo> query = toDoListDbContext.ToDos
                .Include(t => t.Priority)
                .Include(t => t.Status);

            if(filters.HasPriority)
            {
                query = query.Where(t => t.PriorityId == filters.PriorityId);
            }

            if (filters.HasStatus)
            {
                query = query.Where(t => t.StatusId == filters.StatusId);
            }

            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if(filters.IsPast) 
                {
                    query = query.Where(t => t.DueDate < today);
                }
                else if(filters.IsFuture)
                {
                    query = query.Where(t => t.DueDate > today);
                }
                else if (filters.IsToday)
                {
                    query = query.Where(t => t.DueDate == today);
                }
            }

            var tasks = query.OrderBy(t => t.DueDate).ToList();

            return View(tasks);
        }

        // GET: TaskController/Add
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Priorities = toDoListDbContext.Priorities.ToList();
            ViewBag.Statuses = toDoListDbContext.Statuses.ToList();

            var task = new ToDo { StatusId = "notstarted" };

            return View(task);
        }

        // POST: TaskController/Add
        [HttpPost]
        public IActionResult Add(ToDo task)
        {
            if (ModelState.IsValid)
            {
                toDoListDbContext.ToDos.Add(task);
                toDoListDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Priorities = toDoListDbContext.Priorities.ToList();
                ViewBag.Statuses = toDoListDbContext.Statuses.ToList();
                return View(task);
            }

        }

        // Filter Tasks
        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction("Index", new { ID = id });

        }

        // Mark Task Status as 'Completed' and 'In Progress'
        [HttpPost]
        public ActionResult UpdateStatus([FromRoute]string id, ToDo selected)
        {
            selected = toDoListDbContext.ToDos.Find(selected.TaskID)!;

            if (selected != null)
            {
                if (selected.StatusId == "notstarted")
                {
                    selected.StatusId = "inprogress";
                }
                else if (selected.StatusId == "inprogress")
                {
                    selected.StatusId = "completed";
                }
                toDoListDbContext.SaveChanges();
            }

            return RedirectToAction("Index", new { ID = id });
        }

        // Delete 'Completed' Task
        public IActionResult DeleteComplete(string id)
        {
            var toDelete = toDoListDbContext.ToDos.Where(t => t.StatusId == "completed").ToList();

            foreach (var task in toDelete) 
            {
                toDoListDbContext.ToDos.Remove(task);
            }
            toDoListDbContext.SaveChanges();

            return RedirectToAction("Index", new { ID = id });
        }
    }
}