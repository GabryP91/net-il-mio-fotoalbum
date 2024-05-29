using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Il titolo è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il titolo non può superare i 100 caratteri")]
        public string Titolo { get; set; }

        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        [StringLength(255, ErrorMessage = "La descrizione non può superare i 255 caratteri")]
        public string Descrizione { get; set; }
        public string? ImagePath { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime ModifydAt { get; set; }


        //relazione tra la foto e le categorie.(many-to-many)
        [Required(ErrorMessage = "Selezionare almeno una categoria")]
        public List<Category> Categories { get; set; }

        //relazione tra la foto e l'utente che l'ha creata.(one-to-many)
        public string Userid { get; set; } // Foreign key to the user who created the photo
        public IdentityUser? User { get; set; } // Navigation property to the user

        //METODI
        public Photo() {

            CreatedAt = DateTime.Now;
        }

        public Photo(string titolo, string descrizione, string foto, bool visibile, string id) : this()
        {
            this.Titolo = titolo;

            this.Descrizione = descrizione;

            this.ImagePath = foto;

            this.IsVisible = visibile;

            this.Userid = id;

        }
    }
}
