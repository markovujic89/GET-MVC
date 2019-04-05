using GeneralEngineeringTechnologies.Helper;
using GeneralEngineeringTechnologies.Models;
using GeneralEngineeringTechnologies.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeneralEngineeringTechnologies.Controllers
{
    /// <summary>
    /// Complete logic for project management.
    /// </summary>
    public class ProjectController : Controller
    {
        /// <summary>
        /// Inastance of <<see cref="ApplicationDbContext"/>.
        /// </summary>
        private ApplicationDbContext dbContex;

        /// <summary>
        /// Instance of <<see cref="RoleHelper"/>.
        /// </summary>
        private RoleHelper roleHelper;

        
        /// <summary>
        /// Project Controller constructor.
        /// </summary>
        public ProjectController()
        {
            dbContex = new ApplicationDbContext();
            roleHelper = new RoleHelper(dbContex);
        }

        // GET: Project
        /// <summary>
        /// Representation All project with specific parameters.
        /// </summary>
        /// <returns>Page with table all existing projects.</returns>
        [HttpGet]
        public ActionResult ProjectSummary()
        {
            var projectList = dbContex.Projects.Include(ControllerConstants.ProjectManager).Include(ControllerConstants.Tasks).ToList();
            
            if(User.IsInRole(RoleName.Developer))
            {
                return View("ProjectSummaryForDeveloper", projectList);
            }

            return View("ProjectSummary", projectList);
        }
        /// <summary>
        /// Action for project form page. This action is used for adding new project or edit existing project.
        /// </summary>
        /// <returns>Project Form page.</returns>
        public ActionResult ProjectForm()
        {
            var projectViewModel = new ProjectViewModel()
            {
                Project = new Project
                {
                    Tasks = new List<Task>(),
                    Name = string.Empty,
                    Code = string.Empty,
                    ProjectManager = null
                },

                ProjectManagers = roleHelper.GetAllProjectManager(),
                ProjectManager=string.Empty,
                Tasks = new List<Task>()
            };

            return View(projectViewModel);
        }

        /// <summary>
        /// Save Action, for adding and updating project.
        /// </summary>
        /// <param name="projectViewModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save(ProjectViewModel projectViewModel)
        {
            if (!ModelState.IsValid)
            {
                var newProject = new ProjectViewModel
                {
                    Project = projectViewModel.Project,
                    ProjectManagers = roleHelper.GetAllProjectManager(),
                    ProjectManager = projectViewModel.ProjectManager,
                    Tasks=projectViewModel.Tasks

                };

                return View("ProjectForm", newProject);
            }

            Project proj = dbContex.Projects.SingleOrDefault(x => x.Id == projectViewModel.Project.Id);

            if (proj == null)
            {
                // add new project
                proj = AddNewProject(projectViewModel);
            }
            else
            {
                // edit current project
                proj.Name = projectViewModel.Project.Name;
                proj.Code = projectViewModel.Project.Code;
                proj.ProjectManager = roleHelper.GetApplicationUserByName(projectViewModel.ProjectManager);
            }

            dbContex.SaveChanges();

            return RedirectToAction("ProjectSummary", "Project");
        }

        /// <summary>
        /// Action for edit existing project in ProjectSummary.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            Project project = dbContex.Projects.Include(ControllerConstants.ProjectManager).Include(ControllerConstants.Tasks).SingleOrDefault(x => x.Id == id);

            if (project == null)
            {
                return HttpNotFound();
            }
            
            ProjectViewModel viewModel = new ProjectViewModel
            {
                Project = project,
                ProjectManagers = roleHelper.GetAllProjectManager(),
                ProjectManager = project.ProjectManager.UserName,
                Tasks = dbContex.Tasks.Include(ControllerConstants.AssignedUser).Where(x => x.Project.Id == project.Id).ToList()
            };

            return View("ProjectForm", viewModel);
        }

        /// <summary>
        /// Action for delete selected project in summary.
        /// </summary>
        /// <param name="id">Identifier of selected project, for removing (delete).</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            Project project = dbContex.Projects.Include(ControllerConstants.ProjectManager).Include(ControllerConstants.Tasks).SingleOrDefault(x => x.Id == id);

            if (project == null)
            {
                return HttpNotFound();
            }

            if(project.Tasks.Count>0)
            {
                dbContex.Tasks.RemoveRange(project.Tasks);
            }

            dbContex.Projects.Remove(project);

            dbContex.SaveChanges();

            return RedirectToAction("ProjectSummary", "Project");
        }

        /// <summary>
        /// Add new project, if current project doesn't exist in database.
        /// </summary>
        /// <param name="projectViewModel">Instance of <<see cref="ProjectViewModel"/> with data entered in view.</param>
        /// <returns></returns>
        private Project AddNewProject(ProjectViewModel projectViewModel)
        {
            Project proj;
            UserManager<ApplicationUser> userManager = roleHelper.InitialiManager(dbContex);
            proj = projectViewModel.Project;
            var currentUser = userManager.FindByName(projectViewModel.ProjectManager);
            proj.ProjectManager = currentUser;
            proj.Tasks = projectViewModel.Tasks;
            dbContex.Projects.Add(proj);
            return proj;
        }
    }
}