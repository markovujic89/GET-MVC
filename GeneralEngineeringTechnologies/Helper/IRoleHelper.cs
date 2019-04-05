using GeneralEngineeringTechnologies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.Helper
{
    public interface IRoleHelper
    {
        /// <summary>
        /// Chnage User role action.
        /// </summary>
        /// <param name="userEdit">Instance of <see cref="ApplicationUser"/> which role will been changed.</param>
        /// <param name="changedRole">New User role.</param>
        void ChangeUserRole(ApplicationUser userEdit, string changedRole);

        /// <summary>
        /// Get Role name for specific <see cref="ApplicationUser"/>.
        /// </summary>
        /// <param name="user">Instance of <see cref="ApplicationUser"/></param>
        /// <returns>Role Name</returns>
        string GetRoleName(ApplicationUser user);

        /// <summary>
        /// Get all users from database with Project Manager Role.
        /// </summary>
        /// <returns>Collection of users with Project Manager Role.</returns>
        IEnumerable<ApplicationUser> GetAllProjectManager();

        /// <summary>
        /// Get <see cref="ApplicationUser"/> from data base with specific user name.
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <returns><see cref="ApplicationUser"/> with specic user name</returns>
        ApplicationUser GetApplicationUserByName(string userName);

        /// <summary>
        /// Get <see cref="ApplicationUser"/> from data base with specific user Id.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>User name.</returns>
        string GetApplicationUserNameById(string id);

        /// <summary>
        /// Get Collection of Users with Developer Role.
        /// </summary>
        /// <returns>Collection of users with Developer Role.</returns>
        IEnumerable<ApplicationUser> GetAllDevelopers();
    }
}