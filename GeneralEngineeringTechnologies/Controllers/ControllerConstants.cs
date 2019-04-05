using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.Controllers
{
    public static class ControllerConstants
    {
        /// <summary>
        /// ProjectManager value related to FK ProjectManager in Projects table.
        /// </summary>
        internal static string ProjectManager = "ProjectManager";

        /// <summary>
        /// Tasks value related to FK Tasks (collection of Task) in Projects table.
        /// </summary>
        internal static string Tasks = "Tasks";

        /// <summary>
        /// Name of FK value in Taks table related to Assigned User (Developer).
        /// </summary>
        internal static string AssignedUser = "AssignedUser";

        /// <summary>
        /// Not assigned task.
        /// </summary>
        internal static string NotAssigne = "Task is not Assigne";

        /// <summary>
        /// FK Project in Tasks table.
        /// </summary>
        internal static string Project = "Project";

        /// <summary>
        /// Message for deleting busy user with Project Manager role.
        /// </summary>
        internal static string DeleteBusyProjectManager = "Can not delete existing user. Current user with a project manager role works on existing project.";

        /// <summary>
        /// Message for deleting developer assigned on the task.
        /// </summary>
        internal static string DeleteAssignedUserOnTask = "Can not delete existing user. Current user is assigned on the task in progress.";
    }
}