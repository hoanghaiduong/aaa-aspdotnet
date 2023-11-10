using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aaa_aspdotnet.src.Common.DTO
{
    public class JwtPayload
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? RoleId { get; set; }
    }
}
