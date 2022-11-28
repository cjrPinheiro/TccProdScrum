using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PS.Aplication.Dtos;
using PS.Aplication.Interfaces;
using PS.Common.Extensions;
using PS.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Api.Business.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProjectController : ControllerBase
    {
        private readonly IJiraService _jiraService;
        private readonly IProjectService _projectService;

        public ProjectController(IJiraService jiraService, IProjectService projectService)
        {
            _jiraService = jiraService;
            _projectService = projectService;
        }

        [HttpGet("{jiraDomainId}")]
        public async Task<IActionResult> GetProjects(int jiraDomainId)
        {
            try
            {
                int userId = User.GetUserId();

                List<ProjectDto> listProjects = await _projectService.GetProjectsByJiraDomainId(userId, jiraDomainId);
                if (listProjects != null)
                    return Ok(listProjects);
                return BadRequest("Não foi possível realizar a consulta no momento. Tente novamente em breve.");


            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectDto editedProject)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _projectService.ProjectExists(userId, id))
                {
                    var project = await _projectService.UpdateProject(id, editedProject);
                    if (project != null)
                        return Ok(project);
                    return BadRequest("Não foi possível atualizar o projeto no momento. Tente novamente em breve.");
                }
                else
                    return BadRequest("Projeto não existe.");
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }


    }
}
