using GeneralEngineeringTechnologies.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.Models
{
    public class MaximumThreeAssignedTask: ValidationAttribute
    {
        private RoleHelper roleHelper;
        private ApplicationDbContext dbContext;

        public MaximumThreeAssignedTask()
        {
            dbContext = new ApplicationDbContext();
            roleHelper = new RoleHelper(dbContext);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var task = (Task)validationContext.ObjectInstance;

            Project project = task.Project;

            ICollection<Task> taskOnProject = project.Tasks;

            List<ApplicationUser> currentUser = taskOnProject.Select(x => x.AssignedUser).Where(x => x.UserName == task.AssignedUser.UserName).ToList();

            if (currentUser.Count()>3)
            {
                return new ValidationResult("Current user can be assigned maximum on three task");
            }

            return ValidationResult.Success;
        }
    }
}