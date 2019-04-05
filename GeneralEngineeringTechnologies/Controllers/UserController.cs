using GeneralEngineeringTechnologies.Helper;
using GeneralEngineeringTechnologies.Models;
using GeneralEngineeringTechnologies.ViewModel;
using System.Linq;
using System.Web.Mvc;

namespace GeneralEngineeringTechnologies.Controllers
{
    /// <summary>
    /// Class who contain actions for manipulation Application user. 
    /// Only user with Administrator role can modify Application users
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/>.
        /// </summary>
        private ApplicationDbContext dbContex;

        /// <summary>
        /// Instance of <see cref="RoleHelper"/>.
        /// </summary>
        private RoleHelper roleHelper;

        /// <summary>
        /// UserController constructor.
        /// </summary>
        public UserController()
        {
            dbContex = new ApplicationDbContext();
            roleHelper = new RoleHelper(dbContex);
        }

        // GET: User
        /// <summary>
        /// Action for representing all users. Only User with Administrator role can modify users.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles=RoleName.Administrator)]       
        public ActionResult UserSummary()
        {
            var roleUser = dbContex.Roles.Where(x => x.Name.Contains(RoleName.Developer)).FirstOrDefault();
            var developers = dbContex.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleUser.Id)).ToList();

            var usersView = developers.Select(user => new UserRoleViewModel
            {
                Username=user.UserName,
                Email = user.Email,
                RoleName = RoleName.Developer,
                Id=user.Id
                
            }).ToList();

            var roleProjectManager = dbContex.Roles.Where(x => x.Name.Contains(RoleName.ProjectManager)).FirstOrDefault();
            var projectManagers = dbContex.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleProjectManager.Id));

            var projecManagerView = projectManagers.Select(projectManager => new UserRoleViewModel
            {
                Username= projectManager.UserName,
                Email = projectManager.Email,
                RoleName = RoleName.ProjectManager,
                Id = projectManager.Id
            }).ToList();

            if (TempData["projectManager"] != null)
            {
                ViewBag.Message = TempData["projectManager"];
            }

            if(TempData["developer"] !=null)
            {
                ViewBag.Message = TempData["developer"];
            }

            var model = new GroupedUserViewModel { Users = usersView, ProjectManager = projecManagerView };

            return View(model);
        }

        /// <summary>
        /// Application User Form.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns></returns>
        public ActionResult UserForm(string id)
        {
            var user = dbContex.Users.SingleOrDefault(x => x.Id == id);

            ApplicationUserWithRoles userRole = null;
            if (user!=null)
            {
                userRole = new ApplicationUserWithRoles
                {
                    AppUser = user,
                    CurrentRole = roleHelper.GetRoleName(user)
                    
                };
            }

            return View(userRole);
        }

        /// <summary>
        /// Action for saving change on user (change role, name, email..).
        /// </summary>
        /// <param name="usersWithRole"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save(ApplicationUserWithRoles usersWithRole)
        {
            
            if (!ModelState.IsValid)
            {
                var newProject = new ApplicationUser
                {
                    UserName = usersWithRole.AppUser.UserName,
                    Email = usersWithRole.AppUser.Email,
                };

                return View("UserForm", usersWithRole.AppUser);
            }

            ApplicationUser userEdit = dbContex.Users.SingleOrDefault(x => x.Id == usersWithRole.AppUser.Id);

            if (userEdit == null)
            {
                dbContex.Users.Add(userEdit);
            }
            else
            {
                userEdit.UserName = usersWithRole.AppUser.UserName;
                userEdit.Email = usersWithRole.AppUser.Email;
                roleHelper.ChangeUserRole(userEdit, usersWithRole.CurrentRole);
            }

            dbContex.SaveChanges();

            return RedirectToAction("UserSummary", "User");
        }

        /// <summary>
        /// Delete existing user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            ApplicationUser user = dbContex.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            dbContex.Users.Remove(user);

            if (dbContex.Projects.Where(x => x.ProjectManager.UserName == user.UserName).FirstOrDefault()!=null)
            {
                TempData["projectManager"] = ControllerConstants.DeleteBusyProjectManager;
                return RedirectToAction("UserSummary", "User");
            }

            if(dbContex.Tasks.Where(x => x.AssignedUser.UserName == user.UserName).FirstOrDefault() != null)
            {
                TempData["developer"] = ControllerConstants.DeleteAssignedUserOnTask;
                return RedirectToAction("UserSummary", "User");
            }

            dbContex.SaveChanges();

            return RedirectToAction("UserSummary", "User");
        }
    }
}