using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Week8WeekendHomework.Models;

namespace Week8WeekendHomework.Controllers
{
    public class TaskTODOController : Controller
    {
        taskdbmodelDataContext taskdb = new taskdbmodelDataContext();


        //public ActionResult Index()
        //{
        //    return View();
        //}


        public ActionResult Index()
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

            return RedirectToAction("Index");

        }

        [Authorize]
        public ActionResult EditTask(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            TaskTableHomework tsk = TaskTableHomework.GetTaskById(id, taskdb);

            return View(tsk);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditTask([Bind(Include = "TaskID, TaskName, TaskDescription, TaskDueDate, TaskComplete")] TaskTableHomework task)
        {
            TaskTableHomework.EditTask(task, taskdb);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult DeleteTask(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            TaskTableHomework task = TaskTableHomework.GetTaskById(id, taskdb);

            return View(task);
        }

        [Authorize]
        [HttpPost, ActionName("DeleteTask")]
        public ActionResult DeleteActual(int? id)
        {
            TaskTableHomework.DeleteTaskByID(id, taskdb);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult getTasksDueTodayC(int? id)
        {
            TaskTableHomework instance = new TaskTableHomework();
            return View("~/Views/TaskTODO/Index.cshtml", instance.getTasksDueToday(id, taskdb));
        }

        public ActionResult getTasksDueTomorrowC(int? id)
        {
            TaskTableHomework instance = new TaskTableHomework();
            return View("~/Views/TaskTODO/Index.cshtml", instance.getTasksDueTomorrow(id, taskdb));
        }

        public ActionResult getTasksCompletedC()
        {
            TaskTableHomework instance = new TaskTableHomework();
            return View("~/Views/TaskTODO/Index.cshtml", instance.getTasksCompleted(taskdb));
        }

        public ActionResult getTasksOverDueC()
        {
            TaskTableHomework instance = new TaskTableHomework();
            return View("~/Views/TaskTODO/Index.cshtml", instance.getTasksOverDue(taskdb));
        }
    }

    
}