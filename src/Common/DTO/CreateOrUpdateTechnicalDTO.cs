using System.ComponentModel.DataAnnotations;

namespace aaa_aspdotnet.src.Common.DTO
{
    public class CreateOrUpdateTechnicalDTO
    {
    
            [Required]
            [MaxLength(255)]
            public string TechnicalName { get; set; }

            [MaxLength(20)]
            public string Phone { get; set; }

            [MaxLength(20)]
            public string Phone2 { get; set; }

            [MaxLength(255)]
            public string Address { get; set; }

            [MaxLength(255)]
            public string Zalo { get; set; }

            [MaxLength(255)]
            [EmailAddress]
            public string Email { get; set; }

            public bool IsDelete { get; set; }
       
    }
}
