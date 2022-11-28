using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PS.Aplication.Dtos;
using PS.Aplication.Interfaces;
using PS.Application.Interfaces;
using PS.Atlassian.Connector.Models.Requests.TeamMember;
using PS.Common.Extensions;
using PS.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PS.Api.Business.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JiraController : ControllerBase
    {
        private readonly IJiraService _jiraService;
        private readonly IProjectService _projectService;

        public JiraController(IJiraService jiraService, IProjectService projectService)
        {
            _jiraService = jiraService;
            _projectService = projectService;
        }

        [HttpGet("importProjects/{jiraDomainid}")]
        public async Task<IActionResult> ImportProjects(int jiraDomainid)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _jiraService.DomainExists(userId, jiraDomainid))
                {
                    var totalImports = await _jiraService.ImportProjects(userId, jiraDomainid);
                    if (totalImports > 0)
                        return Ok(new
                        {
                            totalImports
                        });
                    return BadRequest("Não existem novos projetos para serem importados.");
                }
                else
                    return this.StatusCode(StatusCodes.Status412PreconditionFailed);
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpGet("syncMembers/{jiraDomainid}")]
        public async Task<IActionResult> SyncMembers(int jiraDomainid)
        {
            try
            {
                int userId = User.GetUserId();
                var projects = await _projectService.GetProjectsByJiraDomainId(userId, jiraDomainid);
                if (projects != null)
                {
                    int totalImports = 0;
                    foreach (var project in projects)
                    {
                        try
                        {
                            totalImports += await _jiraService.ImportTeamMembers(userId, project.Id);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    if (totalImports > 0)
                        return Ok(new
                        {
                            totalImports
                        });
                    return BadRequest("Não existem novos membros para serem importados.");
                }
                else
                    return BadRequest("Domínio não possui projetos cadastrados.");
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpGet("syncSprints/{jiraDomainid}")]
        public async Task<IActionResult> SyncSprints(int jiraDomainid)
        {
            try
            {
                int userId = User.GetUserId();
                var projects = await _projectService.GetProjectsByJiraDomainId(userId, jiraDomainid);
                if (projects != null)
                {
                    int totalImports = 0;
                    foreach (var project in projects)
                    {
                        try
                        {
                            totalImports += await _jiraService.ImportSprints(userId, project.Id);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    if (totalImports > 0)
                        return Ok(new
                        {
                            totalImports
                        });
                    return BadRequest("Não existem novas sprints para serem importadas.");
                }
                else
                    return BadRequest("Domínio não possui projetos cadastrados.");
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }
        [HttpGet("syncStatuses/{jiraDomainid}")]
        public async Task<IActionResult> SyncStatuses(int jiraDomainid)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _jiraService.DomainExists(userId, jiraDomainid))
                {
                    int totalImports = await _jiraService.ImportStatuses(userId, jiraDomainid);

                    if (totalImports > 0)
                        return Ok(new
                        {
                            totalImports
                        });
                    return BadRequest("Não existem novos status para serem importados.");
                }
                else
                    return BadRequest("Domínio não possui projetos cadastrados.");
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpGet("importUsers/{projectid}")]
        public async Task<IActionResult> ImportUsers(int projectId)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _projectService.ProjectExists(userId, projectId))
                {
                    var members = await _jiraService.ImportTeamMembers(userId, projectId);
                    if (members != 0)
                        return Ok(members);
                    return BadRequest("Não existem novos usuários para serem importados no momento. Tente novamente em breve.");
                }
                else
                    return BadRequest("Domínio não existe.");
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpGet("importSprints/{projectid}")]
        public async Task<IActionResult> ImportSprints(int projectId)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _projectService.ProjectExists(userId, projectId))
                {
                    var sprints = await _jiraService.ImportSprints(userId, projectId);
                    if (sprints != 0)
                        return Ok(sprints);
                    return BadRequest("Não existem sprints para se importar no momento. Por favor, tente novamente em breve.");
                }
                else
                    return BadRequest("Sem permissão de acesso ou domínio não está cadastrado");
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpGet("getProjects/{jiraDomainId}")]
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

        [HttpGet("getSprints/{projectId}")]
        public async Task<IActionResult> GetSprints(int projectId)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _projectService.ProjectExists(userId, projectId))
                {
                    List<SprintDto> listSprints = await _jiraService.GetSprintsByProjectId(userId, projectId);
                    if (listSprints != null)
                        return Ok(listSprints);
                    return BadRequest("Não foi possível realizar a consulta no momento. Tente novamente em breve.");
                }
                else
                    return BadRequest("Sprints não encontradas para este usuário.");

            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpGet("GetTeamPoints/{projectid}")]
        public async Task<IActionResult> GetTeamPoints(int projectId, DateTime initialDate, DateTime endDate)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _projectService.ProjectExists(userId, projectId))
                {
                    List<TeamMemberPerformance> listUsers = await _jiraService.GetIssuesByDoneStatuses(userId, projectId, initialDate, endDate);
                    if (listUsers != null)
                        return Ok(listUsers);
                    return BadRequest("Não foi possível realizar a consulta no momento. Tente novamente em breve.");
                }
                else
                    return BadRequest("Projeto não encontrado para este usuário.");
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpGet("GetTeamPointsBySprintId/{projectid}")]
        public async Task<IActionResult> GetTeamPointsBySprintId(int projectId, int sprintId)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _projectService.ProjectExists(userId, projectId))
                {
                    List<TeamMemberPerformance> listUsers = await _jiraService.GetIssuesBySprintId(userId, sprintId);
                    if (listUsers != null)
                        return Ok(listUsers);
                    return BadRequest("Não foi possível realizar a consulta no momento. Tente novamente em breve.");
                }
                else
                    return BadRequest("Projeto não encontrado para este usuário.");
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
