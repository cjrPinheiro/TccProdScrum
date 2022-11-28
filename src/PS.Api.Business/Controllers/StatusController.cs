using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PS.Aplication.Dtos;
using PS.Aplication.Interfaces;
using PS.Common.Extensions;
using PS.Common.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PS.Api.Business.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly IProjectService _projectService;

        public StatusController(IStatusService statusService, IProjectService projectService)
        {
            _statusService = statusService;
            _projectService = projectService;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetStatusesByProjectId(int projectId)
        {

            try
            {
                int userId = User.GetUserId();

                if (await _projectService.ProjectExists(userId, projectId)){
                    List<StatusDto> listStatus = await _statusService.GetStatusesByProjectId(projectId);
                    if (listStatus != null)
                        return Ok(listStatus);
                    return BadRequest("Não foi possível realizar a consulta no momento. Tente novamente em breve.");
                }
                return BadRequest("Não foi possível localizar os status para o projeto solicitado.");
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
