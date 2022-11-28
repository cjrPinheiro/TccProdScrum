using PS.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(UserExistingDto existingDto);
    }
}
