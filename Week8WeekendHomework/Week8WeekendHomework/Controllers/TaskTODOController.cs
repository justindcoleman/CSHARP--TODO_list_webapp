using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Week8WeekendHomework.Models;

namespace Week8WeekendHomework.Controllers
{
    public class TaskTODOController : Controller
    {
        taskdbmodelDataContext taskdb = new taskdbmodelDataContext();


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetTaskList()
        {
            var usr = User.Identity.Name;

            var myTasks = TaskTableHomework.GetMyTasks(taskdb, usr);

            return View(myTasks);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateTask()
        {

            return View();
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult CreateTask([Bind(Include = "TaskID, TaskName, TaskDescription, TaskDueDate, TaskComplete")] TaskTableHomework task)
        {
            TaskTableHomework.CreateTask(task, User.Identity.Name, taskdb);

            return RedirectToAction("GetTaskList");

        }
    }
}