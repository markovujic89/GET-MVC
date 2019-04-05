using GeneralEngineeringTechnologies.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.Helper
{
    /// <summary>
    /// Helper class for operation with ApplicationUsers and Roles.
    /// </summary>
    public class RoleHelper:IRoleHelper
    {
        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/>
        /// </summary>
        private ApplicationDbContext dbContex;

        /// <summary>
        /// Instance of <see cref="UserManager"/>
        /// </summary>
        UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Constructor of RoleHelper.
        /// </summary>
        /// <param name="contex"></param>
        public RoleHelper(ApplicationDbContext contex)
        {
            dbContex = contex;
        }

        /// <summary>
        /// Chnage User role action.
        /// </summary>
        /// <param name="userEdit">Instance of <see cref="ApplicationUser"/> which role will been changed.</param>
        /// <param name="changedRole">New User role.</param>
        public void ChangeUserRole(ApplicationUser userEdit, string changedRole)
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContex));

            var oldUser = userManager.FindById(userEdit.Id);

            string oldRoleName = GetRoleName(oldUser);

            if (oldRoleName != changedRole)
            {
                userManager.RemoveFromRole(userEdit.Id, oldRoleName);
                userManager.AddToRole(userEdit.Id, changedRole);
            }

            dbContex.Entry(userEdit).State = EntityState.Modified;
        }

        /// <summary>
        /// Get Role name for specific <see cref="ApplicationUser"/>.
        /// </summary>
        /// <param name="user">Instance of <see cref="ApplicationUser"/></param>
        /// <returns>Role Name</returns>
        public string GetRoleName(ApplicationUser user)
        {
            string id = user.Roles.SingleOrDefault().RoleId;

            return dbContex.Roles.SingleOrDefault(x => x.Id == id).Name;
        }

        /// <summary>
        /// Get all users from database with Project Manager Role.
        /// </summary>
        /// <returns>Collection of users with Project Manager Role.</returns>
        public IEnumerable<ApplicationUser> GetAllProjectManager()
        {
            var roleUser = dbContex.Roles.Where(x => x.Name.Contains(RoleName.ProjectManager)).FirstOrDefault();
            IEnumerable<ApplicationUser> users = dbContex.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleUser.Id)).ToList();

            return users;
        }

        /// <summary>
        /// Get <see cref="ApplicationUser"/> from data base with specific user name.
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <returns><see cref="ApplicationUser"/> with specic user name</returns>
        public ApplicationUser GetApplicationUserByName(string userName)
        {
            ApplicationUser user = dbContex.Users.SingleOrDefault(x => x.UserName == userName);

            return user;
        }

        /// <summary>
        /// Get <see cref="ApplicationUser"/> from data base with specific user Id.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>User name.</returns>
        public string GetApplicationUserNameById(string id)
        {
            ApplicationUser user = dbContex.Users.SingleOrDefault(x => x.Id == id);

            return user.UserName;
        }

        /// <summary>
        /// Get Collection of Users with Developer Role.
        /// </summary>
        /// <returns>Collection of users with Developer Role.</returns>
        public IEnumerable<ApplicationUser> GetAllDevelopers()
        {
            var roleUser = dbContex.Roles.Where(x => x.Name.Contains(RoleName.Developer)).FirstOrDefault();
            IEnumerable<ApplicationUser> developers = dbContex.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleUser.Id)).ToList();

            return developers;
        }

        internal UserManager<ApplicationUser> InitialiManager(ApplicationDbContext dbContex)
        {
            return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContex));
        }
    }
}