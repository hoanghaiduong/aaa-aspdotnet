using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.Helpers;
using aaa_aspdotnet.src.DAL.Entities;
using aaa_aspdotnet.src.DAL.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;


namespace aaa_aspdotnet.src.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> userRepository;
        private readonly AppDbContext _context;

        ILogger<UserService> log;
        public UserService(IBaseRepository<User> userRepository, ILogger<UserService> log, AppDbContext context)
        {
            this.userRepository = userRepository;
            this.log = log;
            _context = context;
        }

        public async Task<Response> CreateUser(CreateUserDTO dto)
        {
            // Check if the user already exists
            var existingUser = await userRepository.Find(x => x.Username == dto.UserName);

            if (existingUser != null)
            {
                return new Response(HttpStatusCode.Conflict, "User already exists");
            }

            var user = new User()
            {
                Username = dto.UserName,
                Password = dto.Password,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Avatar = dto.Avatar,
                Gender = dto.Gender,
            };
            userRepository.Add(user);
            if (await userRepository.SaveChangesAsync())
            {
                return new Response(HttpStatusCode.BadRequest, "Error");
            }
            return new Response(HttpStatusCode.OK, "OK successfully");

        }

        public async Task<User> GetUserById(string id)
        {
            return await userRepository.GetById(id);
        }
        public async Task<User> GetUserByCondition(string id)
        {
            return await userRepository.FindByCondition(u => u.UserId == id);
        }

     
        public async Task<Response> CreateOrUpdateUserWithHelper(CreateOrUpdateUserDTO dto, string? userId)
        {
           
            try
            {
                var parameters = new Dictionary<string, object>()
                  {
                            { "@UserId", userId??(object)DBNull.Value },
                            { "@Username", dto.UserName?? (object)DBNull.Value },
                                    { "@Password", dto.Password ??(object) DBNull.Value },
                                    { "@Email", dto.Email ??(object) DBNull.Value },
                                    { "@Gender", dto.Gender ??(object) DBNull.Value }, // Sử dụng DBNull.Value nếu dto.Gender là null
                                    { "@Avatar", dto.Avatar ??(object) DBNull.Value },
                                    { "@RefreshToken", (object)dto.RefreshToken ??(object) DBNull.Value }, // Sử dụng DBNull.Value nếu dto.RefreshToken là null
                                    { "@PhoneNumber", dto.PhoneNumber ??(object) DBNull.Value },
                                    { "@RoleId", dto.RoleId ??(object) DBNull.Value },
                                    { "@IsActived", dto.IsActived}, // Sử dụng DBNull.Value nếu dto.IsActived là null
                                    { "@IsDeleted", dto.IsDeleted } // Sử dụng DBNull.Value nếu dto.IsDeleted là null
            };



                var result = SQLHelper.ExecuteStoredProcedure("PSP_CreateOrUpdateUser", parameters);
                var status = result[0].Where(r => r.Key == "Result").FirstOrDefault().Value;
                var message = result[0].Where(r => r.Key == "Status").FirstOrDefault().Value.ToString();
                var data = result[0].Where(r => r.Key == "Data").FirstOrDefault().Value;

                if (!(bool)status)
                {
                    return new Response(HttpStatusCode.BadRequest,message);
                }
                else
                {
                    return new Response(HttpStatusCode.OK, message,result:  JsonConvert.DeserializeObject(data.ToString()));
                }    
              

              

        

                //  return new Response(HttpStatusCode.OK,message, JsonConvert.DeserializeObject(data.ToString()));


            }
            catch (Exception ex)
            {

                return new Response(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
