using AutoMapper;
using GeneralEngineeringTechnologies.DTOs;
using GeneralEngineeringTechnologies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeneralEngineeringTechnologies.Controllers.API
{
    public class ProjectsController : ApiController
    {
        private ApplicationDbContext dbContex;

        public ProjectsController()
        {
            dbContex = new ApplicationDbContext();
        }

        // GET /API/Projects
        [HttpGet]
        public IEnumerable<ProjectDTO> GetProject()
        {
            return dbContex.Projects.ToList().Select(Mapper.Map<Project, ProjectDTO>);
        }

        [HttpGet]
        public IHttpActionResult GetProject(int id)
        {
            var project = dbContex.Projects.SingleOrDefault(x => x.Id == id);

            if(project == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Project, ProjectDTO>(project));
        }

        // POST /API/Projects
        [HttpPost]
        public IHttpActionResult CreateProject(ProjectDTO projectDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var project = Mapper.Map<ProjectDTO, Project>(projectDTO);
            dbContex.Projects.Add(project);
            dbContex.SaveChanges();

            projectDTO.Id = project.Id;

            return Created(new Uri(Request.RequestUri + "/" + project.Id), projectDTO);
        }

        // PUT /API/Projects/1
        [HttpPut]
        public void UpdateUser(int id, ProjectDTO projectDTO)
        {
            if (ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = dbContex.Projects.SingleOrDefault(x => x.Id == id);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Mapper.Map(projectDTO, project);

            dbContex.SaveChanges();
        }

        // DELETE /API/Projects/1
        [HttpDelete]
        public void DeleteUser(int id)
        {
            var project = dbContex.Projects.SingleOrDefault(x => x.Id == id);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            dbContex.Projects.Remove(project);
            dbContex.SaveChanges();
        }
    }
}
