using System;
using System.Collections.Generic;

using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtPayloads = aaa_aspdotnet.src.Common.DTO.JwtPayload;
namespace aaa_aspdotnet.src.BAL.IServices
{
    public interface IJwtService
    {
        // Phương thức để tạo mã JWT dựa trên thông tin người dùng.
        Task<string> GenerateJwtTokenAsync(JwtPayloads payload);

        // Phương thức để xác thực mã JWT và trả về thông tin người dùng.
        Task<JwtPayloads> DecodeJwtToken(string token);
        Task<string> RefreshTokenAsync(JwtPayloads payload);

        Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    }
}
