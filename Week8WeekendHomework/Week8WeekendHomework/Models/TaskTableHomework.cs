using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Week8WeekendHomework.Models
{
    public partial class TaskTableHomework
    {
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
    }
}