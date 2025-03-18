using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{
    public class PasswordResetDto
    {
        [Required]
        public string EmailToId { get; set; }
        [Required]
        public string EmailToName { get; set; }
        [Required]
        public string ResetLink { get; set; }
        [Required]
        public string EmailBody { get; set; } = string.Empty;
    }
}
