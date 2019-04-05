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
    public class TasksController : ApiController
    {
        private ApplicationDbContext dbContex;

        public TasksController()
        {
            dbContex = new ApplicationDbContext();
        }

        // GET /API/Tasks
        [HttpGet]
        public IEnumerable<TaskDTO> GetProject()
        {
            return dbContex.Tasks.ToList().Select(Mapper.Map<Task, TaskDTO>);
        }

        // GET /API/Tasks/1
        [HttpGet]
        public IHttpActionResult GetTask(int id)
        {
            var task = dbContex.Tasks.SingleOrDefault(x => x.Id == id);

            if(task==null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Task, TaskDTO>(task));
        }

        // POST /API/Tasks
        [HttpPost]
        public IHttpActionResult CreateProject(TaskDTO taskDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var task = Mapper.Map<TaskDTO, Task>(taskDTO);
            dbContex.Tasks.Add(task);
            dbContex.SaveChanges();

            taskDTO.Id = task.Id;

            return Created(new Uri(Request.RequestUri + "/" + task.Id), taskDTO);
        }

        // PUT /API/Tasks/1
        [HttpPut]
        public void UpdateUser(int id, TaskDTO taskDTO)
        {
            if (ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var task = dbContex.Tasks.SingleOrDefault(x => x.Id == id);

            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Mapper.Map(taskDTO, task);

            dbContex.SaveChanges();
        }

        // DELETE /API/Tasks/1
        [HttpDelete]
        public void DeleteUser(int id)
        {
            var task = dbContex.Tasks.SingleOrDefault(x => x.Id == id);

            if (task == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            dbContex.Tasks.Remove(task);
            dbContex.SaveChanges();
        }
    }
}
