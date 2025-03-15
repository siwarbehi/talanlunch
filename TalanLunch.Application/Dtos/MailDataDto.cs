using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
{
    public class MailDataDto
    {
        public string EmailToId { get; set; } // Adresse email du destinataire
        public string EmailToName { get; set; } // Nom du destinataire
        public string EmailSubject { get; set; } // Sujet de l'email
        public string EmailBody { get; set; } // Corps de l'email
    }
}
