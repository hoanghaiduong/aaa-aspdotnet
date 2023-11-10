
using System.ComponentModel.DataAnnotations;

namespace aaa_aspdotnet.src.Common.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }


    }
}
