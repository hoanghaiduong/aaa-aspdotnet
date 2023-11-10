using aaa_aspdotnet.src.BAL.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JwtPayloads = aaa_aspdotnet.src.Common.DTO.JwtPayload;

namespace aaa_aspdotnet.src.BAL.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _jwtAccessSecret;
        private readonly string _jwtRefreshSecret;
        private readonly string _audience;
        private readonly string _issuer;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtAccessSecret = _configuration.GetSection("JWT:Access_Secret").Value;
            _jwtRefreshSecret = _configuration.GetSection("JWT:Refresh_Secret").Value;
            _audience = _configuration.GetSection("JWT:ValidAudience").Value;
            _issuer = _configuration.GetSection("JWT:ValidIssuer").Value;

        }

        //Hàm để giải mã và trích xuất thông tin từ mã JWT
        public async Task<ClaimsPrincipal> GetPrincipals(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtAccessSecret);

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);

                // Retrieve the token's expiration time
                var expirationTime = validatedToken.ValidTo;

                // Calculate the remaining time until token expiration
                var timeUntilExpiration = expirationTime - DateTime.UtcNow;

                return principal;
            }
            catch (Exception)
            {
                // Token validation failed
            }

            return null;
        }

        public async Task<bool> ValidateAccessToken(string token)
        {
        
            try
            {
                var principal = await GetPrincipals(token);
                   if(principal == null)
                    {
                        return false;
                    }
                return true;
            }
            catch (Exception)
            {
                // Token validation failed
            }

            return false;
        }

        public async Task<JwtPayloads> DecodeJwtToken(string token)
        {
            try
            {
                if(token==null)
                {
                    throw new UnauthorizedAccessException();
                }    
                var principal = await GetPrincipals(token);
                    
                if (principal != null)
                {
                    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var username = principal.FindFirst(ClaimTypes.Name)?.Value;
                    var roleId = principal.FindFirst(ClaimTypes.Role)?.Value;

                    // Build an object containing the information to be returned
                    return new JwtPayloads()
                    {
                        Id = userId,
                        UserName = username,
                        RoleId = roleId,
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle the error and return a response or log the exception
            }

            return null;
        }
        public async Task<string> RefreshTokenAsync(JwtPayloads payload)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtRefreshSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _audience,
                Issuer = _issuer,
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, payload.Id.ToString()),
                new Claim(ClaimTypes.Name, payload.UserName.ToString()),
                new Claim(ClaimTypes.Role, payload.RoleId.ToString()),
            // Add any other claims you want to include in the refreshed token
        }),
                Expires = DateTime.UtcNow.AddDays(7), // Extend the expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var refreshedToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(refreshedToken).ToString();
        }

        public async Task<string> GenerateJwtTokenAsync(JwtPayloads payload)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtAccessSecret);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _audience,
                Issuer = _issuer,
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, payload?.Id.ToString()),
                new Claim(ClaimTypes.Name, payload?.UserName.ToString()),
                new Claim(ClaimTypes.Role, payload?.RoleId.ToString()),

                // Các claim khác bạn muốn thêm vào mã JWT
            }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token).ToString();

        }


        public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtRefreshSecret); // Use the secret key for refresh tokens
            try
            {
                SecurityToken validatedToken;
                tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out validatedToken);
                return true;
                // Optionally, check if the refresh token is valid in your database
                //var refreshTokenExistsInDatabase = await CheckIfRefreshTokenExistsAsync(refreshToken);

                //if (refreshTokenExistsInDatabase)
                //{
                //    return true;
                //}
            }
            catch (Exception)
            {
                // Token validation failed
            }

            return false;
        }
       

    }
}
