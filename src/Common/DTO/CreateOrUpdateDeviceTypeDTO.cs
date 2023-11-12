using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aaa_aspdotnet.src.Common.DTO
{
    public class CreateOrUpdateDeviceTypeDTO
    {
        [Required]
        [MaxLength(255)]
        public string TypeName { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; } =false;
    }
}
