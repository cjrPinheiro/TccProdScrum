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
using System.Text;
using System.Threading.Tasks;

namespace PS.Api.Business.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JiraDomainController : ControllerBase
    {

        private readonly IJiraDomainService _domainService;

        public JiraDomainController(IJiraDomainService jiraDomainService)
        {
            _domainService = jiraDomainService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
               int userId = User.GetUserId();

                List<JiraDomainDto> listDomains = await _domainService.GetDomainsByUserId(userId);
                if (listDomains != null)
                    return Ok(listDomains);
                return BadRequest("Não foi possível obter os domínios no momento. Tente novamente em breve.");


            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JiraDomainEditedDto domain)
        {
            try
            {
                int userId = User.GetUserId();
                if (!await _domainService.DomainExists(userId, domain.BaseUrl))
                {
                    var newDomain = await _domainService.AddDomainAsync(userId, domain);
                    if (newDomain != null)
                        return Created("Dominio cadastrado com sucesso !", newDomain);
                    return BadRequest("Domninio não pode ser cadastrado no momento. Tente novamente em breve.");
                }
                else
                    return BadRequest("Dominio ja cadastrado para este usuário.");
            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] JiraDomainEditedDto domain)
        {
            try
            {
                JiraDomainDto updatedDomain = await _domainService.UpdateDomain(User.GetUserId(), id, domain);
                if (updatedDomain != null)
                {
                    return Ok(updatedDomain);
                }
                return BadRequest("The User couldn't be updated at this time. Please try again soon.");

            }
            catch (Exception e)
            {
                var error = CommonFunctions.GenerateError("Ocorreu um erro interno na aplicação.", e);
                //implementar log
                return this.StatusCode(StatusCodes.Status500InternalServerError, error);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted = await _domainService.DeleteDomain(User.GetUserId(), id);
                if(deleted)
                    return Ok();
                return BadRequest("Não foi possível efetuar a exclusão no momento. Tente novamente em breve.");

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
