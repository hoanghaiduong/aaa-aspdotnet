using aaa_aspdotnet.src.BAL.IServices;
using aaa_aspdotnet.src.Common.DTO;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.Common.shared;
using aaa_aspdotnet.src.DAL.Entities;
using aaa_aspdotnet.src.DAL.IRepositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
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

        public async Task<User> CreateOrUpdateUserWithHelper(CreateOrUpdateUserDTO dto, string? userId)
        {
            try
            {
                var userIdParam = new SqlParameter("@UserId", SqlDbType.NVarChar, 128);

                if (!string.IsNullOrEmpty(userId))
                {
                    userIdParam.Value = userId; // Use userId for update operation
                }
                else
                {
                    userIdParam.Direction = ParameterDirection.Output;
                }
                SqlParameter[] parameters = new SqlParameter[]
                {
                    userIdParam,
                    new SqlParameter("@Username", SqlDbType.NVarChar, 255) { Value = dto.UserName ?? (object)DBNull.Value },
                    new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = dto.Password ?? (object)DBNull.Value  },
                    new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = dto.Email ?? (object)DBNull.Value  },
                    new SqlParameter("@Gender", SqlDbType.Bit) { Value = dto.Gender ?? (object)DBNull.Value  },
                    new SqlParameter("@Avatar", SqlDbType.NVarChar, 255) { Value = dto.Avatar ?? (object)DBNull.Value  },
                    new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = dto.Address ?? (object)DBNull.Value  },
                    new SqlParameter("@Zalo", SqlDbType.NVarChar, 100) { Value = dto.Zalo ?? (object)DBNull.Value  },
                    new SqlParameter("@PhoneNumber2", SqlDbType.NVarChar, 100) { Value = dto.PhoneNumber2 ?? (object)DBNull.Value  },
                    new SqlParameter("@RefreshToken", SqlDbType.NVarChar, 255) { Value = dto.RefreshToken ?? (object)DBNull.Value  },
                    new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 15) { Value = dto.PhoneNumber ?? (object)DBNull.Value  },
                    new SqlParameter("@RoleId", SqlDbType.NVarChar, 128) { Value = dto.RoleId ?? (object)DBNull.Value  },
                    new SqlParameter("@IsActived", SqlDbType.Bit) { Value = dto.IsActived },
                    new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = dto.IsDeleted },
                };


                // Define the list of SQL parameters

                // Execute the stored procedure with input and output parameters
                var excute = await _context.Database.ExecuteSqlRawAsync("EXEC PSP_CreateOrUpdateUser @UserId OUTPUT, @Username, @Password, @Email, @Gender, @Avatar, @Address, @Zalo, @PhoneNumber2, @RefreshToken, @PhoneNumber, @RoleId, @IsActived, @IsDeleted", parameters);
                //  return new Response(HttpStatusCode.OK,message, JsonConvert.DeserializeObject(data.ToString()));
                // Get the DeviceID from the output parameter
                Console.WriteLine(excute.ToString());
                // Lấy UserId từ tham số đầu ra
                var userIdAfter = userIdParam.Value as string;


                // Retrieve the updated/inserted device based on the returned DeviceID
                var updatedUser = await _context.Users.Where(u => u.UserId == userIdAfter).SingleAsync();
                if (updatedUser == null)
                {
                    throw new Exception("Tạo người dùng thất bại ! ");
                }
                return updatedUser;

            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("SQL Error: " + sqlEx.Message);

                // Xử lý ngoại lệ từ cơ sở dữ liệu
                throw sqlEx;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

                throw ex;
                // Xử lý các ngoại lệ khác (không phải từ SQL)
            }
        }





        public PagedList<User> GetUsers(PaginationFilterDto dto)
        {

            try
            {
                FormattableString sqlQuery = $"EXEC PSP_User_Select @UserId=NULL";
                var result = SharedClass.GetDataEntitiesWithPaging<User>(
                    _context,
                    sqlQuery,
                    dto);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
