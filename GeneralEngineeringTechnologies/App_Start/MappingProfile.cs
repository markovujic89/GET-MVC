using AutoMapper;
using GeneralEngineeringTechnologies.DTOs;
using GeneralEngineeringTechnologies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeneralEngineeringTechnologies.App_Start
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Project, ProjectDTO>();
            Mapper.CreateMap<ProjectDTO, Project>();
            Mapper.CreateMap<TaskDTO, Task>();
            Mapper.CreateMap<Task, TaskDTO>();
        }
    }
}