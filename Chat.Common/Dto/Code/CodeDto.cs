using System.ComponentModel.DataAnnotations;

namespace Chat.Common.Dto.Code
{
    public class CodeDto
    {
        [Required]
        [StringLength(6,ErrorMessage = "Code incorrect")]
        public string Code { get; set; }
    }
}