using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Configurations
{
    public class MailSettings
    {
        public required string SenderName { get; set; }
        public required string Server { get; set; }
        public required string SenderEmail { get; set; }
        public int Port { get; set; }            // Le port SMTP (ex : 587)

        public required string UserName { get; set; }
        public required string Password { get; set; }

    }
}
