using AutoMapper;
using PS.Aplication.Dtos;
using PS.Aplication.Interfaces;
using PS.Domain.Entities;
using PS.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Aplication.Services
{
    public class JiraDomainService : IJiraDomainService
    {
        private readonly IJiraDomainPersist _domainRepository;

        private readonly IMapper _mapper;

        public JiraDomainService(IMapper mapper, IJiraDomainPersist domainPersist)
        {
            _domainRepository = domainPersist;
            _mapper = mapper;
        }

        public async Task<JiraDomainDto> AddDomainAsync(int userId, JiraDomainEditedDto domain)
        {
            try
            {
                JiraDomain jiraDomain = _mapper.Map<JiraDomain>(domain);
                jiraDomain.UserId = userId;
                await _domainRepository.AddAsync(jiraDomain);

                if (await _domainRepository.SaveChangesAsync())
                {
                    var result = await _domainRepository.GetByIdAsync(userId, jiraDomain.Id);
                    return _mapper.Map<JiraDomainDto>(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<bool> DeleteDomain(int userId, int id)
        {
            try
            {
                var domain = await _domainRepository.GetByIdAsync(userId, id);
                if (domain != null)
                {
                    _domainRepository.Delete(domain);
                    if (await _domainRepository.SaveChangesAsync())
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }

        public async Task<bool> DomainExists(int userId, int domainId)
        {
            try
            {
                return await _domainRepository.GetByIdAsync(userId, domainId) != null;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> DomainExists(int userId, string baseUrl)
        {
            try
            {
                return await _domainRepository.GetByUrlAsync(userId, baseUrl) != null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<JiraDomainDto>> GetDomainsByUserId(int userId)
        {
            try
            {
                List<JiraDomain> res = await _domainRepository.GetByUserIdAsync(userId);
                if (res != null)
                    return _mapper.Map<List<JiraDomainDto>>(res);
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        public async Task<JiraDomainDto> UpdateDomain(int userId, int domainId, JiraDomainEditedDto editedDomain)
        {
            try
            {
                var domain = await _domainRepository.GetByIdAsync(userId, domainId);
                if (domain == null) return null;

                editedDomain.Id = domain.Id;
                if (string.IsNullOrWhiteSpace(editedDomain.ApiKey))
                    editedDomain.ApiKey = domain.ApiKey;
                if (string.IsNullOrWhiteSpace(editedDomain.BaseUrl))
                    editedDomain.BaseUrl = domain.BaseUrl;
                _mapper.Map(editedDomain, domain);

                _domainRepository.Update(domain);

                if (await _domainRepository.SaveChangesAsync())
                {
                    var upDomain = await _domainRepository.GetByIdAsync(userId, domainId);
                    var mapRes = _mapper.Map<JiraDomainDto>(upDomain);
                    return mapRes;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
    }
}
