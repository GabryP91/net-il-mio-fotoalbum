using Microsoft.AspNetCore.Identity;
using NuGet.Packaging.Signing;

namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        public int id { get; set; }
        public string Email { get; set; }
        public string Corpo { get; set; }
        public DateTime CreatedAt { get; set; }

        //relazione tra il messaggio e l'amministratore di riferimento.(one-to-many)
        public string Adminid { get; set; } // Foreign key to the administrator
        public IdentityUser? Admin { get; set; } // Navigation property to the administrator


        //METODI
        public Message()
        {
            CreatedAt = DateTime.Now;
        }

        public Message(string mail, string text, string id) : this()
        {
            this.Corpo = mail;
            this.Corpo = text;
            this.Adminid = id;
        }
    }
}
