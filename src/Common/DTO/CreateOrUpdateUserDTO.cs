using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aaa_aspdotnet.src.Common.DTO
{
    public class CreateOrUpdateUserDTO
    {

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        public string? Zalo { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string RefreshToken { get; set; }
        public string? RoleId { get; set; }
        public bool IsActived { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
