using System.ComponentModel.DataAnnotations;

namespace aaa_aspdotnet.src.Common.DTO
{
    public class CreateOrUpdateDeviceDTO
    {
        public int DeviceID { get; set; }

        [Required]
        public int TypeID { get; set; }

        [Required]
        [MaxLength(255)]
        public string DeviceName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Color { get; set; }

        [MaxLength(255)]
        public string Descriptions { get; set; }

        [MaxLength(255)]
        public string QRCode { get; set; }

        [Required]
        public bool IsDelete { get; set; }

        [Required]
        public int FacID { get; set; }
    }
}
