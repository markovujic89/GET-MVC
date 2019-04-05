using GeneralEngineeringTechnologies.Helper;
using GeneralEngineeringTechnologies.Models;
using GeneralEngineeringTechnologies.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeneralEngineeringTechnologies.Controllers
{
    [AllowAnonymous()]
    public class TaskController : Controller
    {        
        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/>
        /// </summary>
        private ApplicationDbContext dbContex;

        /// <summary>
        /// Instance of <see cref="RoleHelper"/>
        /// </summary>
        private RoleHelper roleHelper;

        /// <summary>
        /// Constructor of <see cref="TaskController"/>
        /// </summary>
        public TaskController()
        {
            dbContex = new ApplicationDbContext();
            roleHelper = new RoleHelper(dbContex);
        }

        public ActionResult TaskList()
        {
            
            var allTask = dbContex.Tasks.Include(ControllerConstants.Project).Include(ControllerConstants.AssignedUser).ToList();

            if (HttpContext.User.IsInRole(RoleName.Developer))
            {
                IEnumerable<Task> developerTaskList = GetDeveloperTaskList();
                return View("TaskListForUser", developerTaskList);
            }
            else
            { 
                return View("TaskList",allTask);
            }
        }

        public ActionResult TaskForm()
        {
            ViewBag.Users = roleHelper.GetAllDevelopers().Select(x => x.UserName);
            ViewBag.Projects = dbContex.Projects.Select(x => x.Name).ToList();
            ViewBag.Proggres = new List<int>(3) { 0, 50, 100 };

            TaskViewModel viewModel = new TaskViewModel
            {
                Task = EmptyTask(),
                UserName = string.Empty,
                ProjectName = string.Empty
            };

            if (HttpContext.User.IsInRole(RoleName.Developer))
            {
                return View("TaskFormForUser", viewModel);
            }

            return View("TaskForm", viewModel);
        }

        

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save(TaskViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("TaskForm", viewModel);
            }

            Task taskDB = dbContex.Tasks.Include(ControllerConstants.AssignedUser).SingleOrDefault(x => x.Id == viewModel.Task.Id);

            if (taskDB == null)
            {
                taskDB = viewModel.Task;
                SetProjectAndUser(viewModel, taskDB);

                dbContex.Tasks.Add(taskDB);
            }
            else
            {
                UpdateTask(viewModel, taskDB);
            }

            dbContex.SaveChanges();

            return RedirectToAction("TaskList", "Task");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Users = roleHelper.GetAllDevelopers().Select(x => x.UserName);
            ViewBag.Projects = dbContex.Projects.Select(x => x.Name).ToList();
            ViewBag.Proggres = new List<int>(3) { 0, 50, 100 };

            Task task = dbContex.Tasks.Include(ControllerConstants.AssignedUser).Include(ControllerConstants.Project).SingleOrDefault(x => x.Id == id);

            if (task == null)
            {
                return HttpNotFound();
            }


            TaskViewModel viewModel = new TaskViewModel
            {
                Task = task,
                ProjectName = task.Project.Name,
                UserName = CheckUserName(task)
            };

            if(HttpContext.User.IsInRole(RoleName.Developer))
            {
                return View("TaskFormForUser", viewModel);
            }

            return View("TaskForm",viewModel);
        }

        /// <summary>
        /// Delete task action.
        /// </summary>
        /// <param name="id">Task idetifier.</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            Task task = dbContex.Tasks.Include(ControllerConstants.AssignedUser).Include(ControllerConstants.Project).SingleOrDefault(x => x.Id == id);

            if (task == null)
            {
                return HttpNotFound();
            }

            dbContex.Tasks.Remove(task);

            dbContex.SaveChanges();

            return RedirectToAction("TaskList", "Task");
        }

        /// <summary>
        /// Update existing task.
        /// </summary>
        /// <param name="viewModel">Input data form TaksForm, which updated current task.</param>
        /// <param name="taskDB">Updating Task.</param>
        private void UpdateTask(TaskViewModel viewModel, Task taskDB)
        {
            taskDB.Deadline = viewModel.Task.Deadline;
            taskDB.Description = viewModel.Task.Description;
            taskDB.Name = viewModel.Task.Name;
            taskDB.Status = viewModel.Task.Status;
            taskDB.Progress = viewModel.Task.Progress;
            CheckAssigneUser(taskDB, viewModel);
        }

        /// <summary>
        /// Set appropriate value (project and assigned user) for newly created task.
        /// </summary>
        /// <param name="viewModel">Task ViewModel that contains all data form TaskForm, for creating new Task.</param>
        /// <param name="taskDB">Newly created task.</param>
        private void SetProjectAndUser(TaskViewModel viewModel, Task taskDB)
        {
            taskDB.AssignedUser = dbContex.Users.SingleOrDefault(x => x.UserName == viewModel.UserName);
            taskDB.Project = dbContex.Projects.SingleOrDefault(x => x.Name == viewModel.ProjectName);
        }

        /// <summary>
        /// Check Assigned User on Task, and modify if it is necessary.
        /// </summary>
        /// <param name="taskDB">Current Task.</param>
        /// <param name="viewModel">Task ViewModel, contains all data for updating current task.</param>
        private void CheckAssigneUser(Task taskDB, TaskViewModel viewModel)
        {
            if(taskDB.AssignedUser == null && viewModel.UserName==ControllerConstants.NotAssigne)
            {
                return;
            }
            else if(taskDB.AssignedUser == null && viewModel.UserName != ControllerConstants.NotAssigne)
            {
                ApplicationUser newUser = roleHelper.GetApplicationUserByName(viewModel.UserName);

                if (newUser != null)
                {
                    taskDB.AssignedUser = newUser;
                }
            }
            else if(viewModel.UserName != taskDB.AssignedUser.UserName)
            {
                ApplicationUser newUser = roleHelper.GetApplicationUserByName(viewModel.UserName);

                if (newUser != null)
                {
                    taskDB.AssignedUser = newUser;
                }
            }
        }

        /// <summary>
        /// Chek is task assigne on User
        /// </summary>
        /// <param name="task">Reference of <see cref="Task"/></param>
        /// <returns>User name if task is assigne, or "Task is not Assigne", if task don't have a developer.</returns>
        private string CheckUserName(Task task)
        {
            if(task.AssignedUser != null)
            {
                return task.AssignedUser.UserName;
            }
            else
            {
                return ControllerConstants.NotAssigne;
            }

        }

        /// <summary>
        /// Get All task where current developer is assigned and all unassigned task.
        /// </summary>
        /// <returns>Collection of all unassigned task, and task who is assigned on current user.</returns>
        private IEnumerable<Task> GetDeveloperTaskList()
        {
            List<Task> allNotAssignedTask = dbContex.Tasks.Include(ControllerConstants.Project).Include(ControllerConstants.AssignedUser).Where(x=>x.AssignedUser == null).ToList();
            List<Task> allCurrentUserTask = dbContex.Tasks.Include(ControllerConstants.Project).Include(ControllerConstants.AssignedUser).Where(x => x.AssignedUser.UserName == User.Identity.Name).ToList();

            allCurrentUserTask.AddRange(allNotAssignedTask);

            return allCurrentUserTask;
        }

        /// <summary>
        /// Empty Task creator.
        /// </summary>
        /// <returns>Empty task.</returns>
        private static Task EmptyTask()
        {
            return new Task
            {
                AssignedUser = new ApplicationUser(),
                Project = new Project(),
                Name = string.Empty,
                Deadline = DateTime.MinValue,
                Progress = 0,
                Description = string.Empty,
                Status = string.Empty
            };
        }
    }
}