
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aaa_aspdotnet.src.BAL.IServices
{
    public interface IAuthService
    {
        Task<bool> CheckRole(string token);
      Task<string> GetToken(HttpRequest request);
        Task<bool> UpdateUserRefreshToken(string userId, string refreshToken);

    }
}
