using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aaa_aspdotnet.src.Common.DTO
{
    public class CreateUserDTO
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public bool? Gender { get; set; }
        public string? Avatar { get; set; }
        [Required]
        public string? RoleId { get; set; }
    }
}
