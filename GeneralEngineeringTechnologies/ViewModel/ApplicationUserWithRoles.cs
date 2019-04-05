using GeneralEngineeringTechnologies.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.ViewModel
{
    /// <summary>
    /// View Model class for User and Application Role (Administrator, Project Manager and Developer).
    /// </summary>
    public class ApplicationUserWithRoles
    {
        /// <summary>
        /// Instance of <see cref="ApplicationUser"/>
        /// </summary>
        public ApplicationUser AppUser { get; set; }

        /// <summary>
        /// Current Role name.
        /// </summary>
        public string CurrentRole { get; set; }

        /// <summary>
        /// User Roles.
        /// </summary>
        public List<string> Roles = new List<string>
        {
            RoleName.ProjectManager,
            RoleName.Developer
        };
    }
}