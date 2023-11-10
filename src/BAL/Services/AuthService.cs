using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.DAL.Entities;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace aaa_aspdotnet.src.BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        public AuthService(AppDbContext context,IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<bool> CheckRole(string token)
        {
            var decodeToken = _jwtService.DecodeJwtToken(token);
            var userWithRole = await _context.Users
              .Include(u => u.Role) // Eager loading the role
              .FirstOrDefaultAsync(u=>u.UserId==decodeToken.Result.Id);
          if(userWithRole.Role.Name=="ADMIN")
            {
                return true;
            }    
            return false;
            
        }

        public async Task<string> GetToken(HttpRequest request)
        {
       
                string authorizationHeader = request.Headers["Authorization"];
                if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer "))
                {
                    return null;
                }
                string idToken = authorizationHeader.Substring("Bearer ".Length);
                return idToken;
          
        }

        public async Task<bool> UpdateUserRefreshToken(string userId, string refreshToken)
        {
            var check = await _context.Database.ExecuteSqlRawAsync("PSP_UpdateUserRefreshToken {0}, {1}", userId, refreshToken);
            if (check == 0)
            {
                return false;
            }
            return true;
        }
    }
}
