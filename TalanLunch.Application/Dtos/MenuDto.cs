using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{

        public class MenuDto
        {
            [Required(ErrorMessage = "La description du menu est obligatoire.")]
            public string? MenuDescription { get; set; }
        }

    
}
