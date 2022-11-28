using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PS.Aplication.Interfaces;
using PS.Aplication.Models;
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
    public class DashboardController : ControllerBase
    {
        private readonly IJiraDomainService _jiraDomainService;
        private readonly IDashboardService _dashboardService;
        public DashboardController(IJiraDomainService jiraDomainService, IDashboardService dashboardService)
        {
            _jiraDomainService = jiraDomainService;
            _dashboardService = dashboardService;
        }
        [HttpGet("{jiraDomainid}")]
        public async Task<ActionResult<GenericChart>> GetProjectsOverview(int jiraDomainid, DateTime initialDate, DateTime endDate)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _jiraDomainService.DomainExists(userId, jiraDomainid))
                {
                    GenericChart chart = await _dashboardService.GenProjectsOverviewChart(userId, jiraDomainid, initialDate, endDate);
                    if (chart != null)
                        return Ok(chart);
                    return BadRequest("Não foi possível gerar o gráfico no momento. Tente novamente em breve");
                }
                else
                    return BadRequest("");
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }

        [HttpGet("{jiraDomainid}/memberChart")]

        public async Task<ActionResult<GenericChart>> GetMemberChart(int jiraDomainid, int memberId, int projectId, DateTime initialDate, DateTime endDate)
        {
            try
            {
                int userId = User.GetUserId();

                if (await _jiraDomainService.DomainExists(userId, jiraDomainid))
                {
                    GenericChart chart = await _dashboardService.GenMemberAvgHistoryChart(userId, projectId, initialDate, endDate);
                    if (chart != null)
                        return Ok(chart);
                    return BadRequest("Não foi possível gerar o gráfico no momento. Tente novamente em breve");
                }
                else
                    return BadRequest("Domínio não existe ou não pertence ao usuário.");
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
