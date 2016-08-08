using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Week8WeekendHomework.Models
{
    public partial class TaskTableHomework
    {
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? FormattedDate
        {
            get
            {
                return Convert.ToDateTime(this.TaskDueDate);
            }
            set
            {
                this.TaskDueDate = value;
            }
        }

        public static IEnumerable<TaskTableHomework> GetMyTasks(taskdbmodelDataContext taskdb, string userName)
        {
            var myTasks = from tskUsr in taskdb.TaskUserLogins
                          join tsk in taskdb.TaskTableHomeworks on tskUsr.TaskID equals tsk.TaskID
                          where tskUsr.UserName == userName
                          select tsk;

            return myTasks;
        }

        public static void CreateTask(TaskTableHomework task, string usrName, taskdbmodelDataContext taskdb)
        {
            taskdb.TaskTableHomeworks.InsertOnSubmit(task);
            taskdb.SubmitChanges();

            var taskID = (from t in taskdb.TaskTableHomeworks
                          where t.TaskName == task.TaskName
                          select t.TaskID).FirstOrDefault();

            TaskUserLogin tskUser = new TaskUserLogin();

            tskUser.UserName = usrName;
            tskUser.TaskID = taskID;

            taskdb.TaskUserLogins.InsertOnSubmit(tskUser);
            taskdb.SubmitChanges();
        }

        public static void EditTask(TaskTableHomework task, taskdbmodelDataContext taskdb)
        {
            var orgTask = (from t in taskdb.TaskTableHomeworks
                           where t.TaskID == task.TaskID
                           select t).FirstOrDefault();

            orgTask.TaskName = task.TaskName;
            orgTask.TaskDescription = task.TaskDescription;
            orgTask.TaskDueDate = task.TaskDueDate;
            orgTask.TaskComplete = task.TaskComplete;
            taskdb.SubmitChanges();
        }

        public static TaskTableHomework GetTaskById(int? id, taskdbmodelDataContext taskdb)
        {
            TaskTableHomework tsk = (from t in taskdb.TaskTableHomeworks
                                     where t.TaskID == id
                                     select t).FirstOrDefault();

            return tsk;
        }

        public static void DeleteTaskByID(int? id, taskdbmodelDataContext taskdb)
        {
            var TaskUsersWithSameId = from tu in taskdb.TaskUserLogins
                                      where tu.TaskID == id
                                      select tu;

            taskdb.TaskUserLogins.DeleteAllOnSubmit(TaskUsersWithSameId);
            taskdb.SubmitChanges();


            TaskTableHomework task = (from t in taskdb.TaskTableHomeworks
                                      where t.TaskID == id
                                      select t).FirstOrDefault();

            taskdb.TaskTableHomeworks.DeleteOnSubmit(task);
            taskdb.SubmitChanges();
        }

        public List<TaskTableHomework> getTasksDueToday(int? id, taskdbmodelDataContext taskdb)
        { // compare all tasks' .duedates and return a list of the ones due today

            var TasksDueToday = (from tdd in taskdb.TaskTableHomeworks
                                 where tdd.TaskDueDate == DateTime.Now.Date
                                 select tdd).ToList();
            return TasksDueToday;
        }

        public List<TaskTableHomework> getTasksDueTomorrow(int? id, taskdbmodelDataContext taskdb)
        { // compare all tasks' .duedates and return a list of the ones due tomorrow
            DateTime DateTimeVar = DateTime.Now.AddDays(1);

            var TasksDueTomorrow = (from tdd in taskdb.TaskTableHomeworks
                                    where tdd.TaskDueDate == DateTimeVar
                                    select tdd).ToList();
            return TasksDueTomorrow;
        }

        public List<TaskTableHomework> getTasksCompleted(taskdbmodelDataContext taskdb)
        { // compare all tasks' completed bit and return a list of the ones set to 1

            List<TaskTableHomework> returnList = (from v in taskdb.TaskTableHomeworks
                                                  where v.TaskComplete == true
                                                  select v).ToList();
            return returnList;
        }

        public List<TaskTableHomework> getTasksOverDue(taskdbmodelDataContext taskdb)
        { // compare all tasks' completed bit and return a list of the ones set to 1

            //List<TaskTableHomework> returnList = (from v in taskdb.TaskTableHomeworks
            //                                      where DbFunctions.TruncateTime(v.TaskDueDate) < DbFunctions.TruncateTime(DateTime.Now) && v.TaskComplete != true
            //                                      select v).ToList();

            List<TaskTableHomework> returnList = (from v in taskdb.TaskTableHomeworks
                                                   where v.TaskDueDate < DateTime.Now.Date && v.TaskComplete != true
                                                   select v).ToList();

            return returnList;
        }
    }
}