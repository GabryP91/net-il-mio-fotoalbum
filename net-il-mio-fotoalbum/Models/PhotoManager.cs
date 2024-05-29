using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Context;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoManager
    {
        //funzione per verificare che la tabella photo sia popolata
        public static int CountAllPhotos()
        {
            using (PhotoContext db = new PhotoContext())
            {
                return db.Photo.Count();
            }

        }

        //funzione che restituisce una lista di foto che hanno lo stesso nome di quello passato
        public static List<Photo> GetPhotoByTitle(string data)
        {
            using (PhotoContext db = new PhotoContext())
            {

                //restituiscimi una lista di foto che hanno lo stesso nome di quello che passo
                List<Photo> photos = db.Photo.Where(p => p.Titolo.Contains(data) && p.IsVisible == true).Include(c => c.Categories).ToList();

                // Carica separatamente l'attributo UserName per ciascuna foto
                foreach (var photo in photos)
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == photo.Userid);

                    if (user != null)
                    {
                        //associo alla foto un nuovo oggetto IdentityUser con solo il campo mail
                        photo.User = new IdentityUser { Email = user.Email };
                    }
                }

                return photos;
            }
        }

        //funzione che restituisce un oggetto photo in base all'id che riceve
        public static Photo GetPhotoById(int data)
        {
            using (PhotoContext db = new PhotoContext())
            {

                //restituiscimi un oggetto photo con stesso id e categorie associate
                Photo photo = db.Photo.Where(p => p.id == data).Include(c => c.Categories).FirstOrDefault();

                return photo;
            }
        }


        //funzione per una lista di foto con annesse categorie e user
        public static List<Photo> GetAllPhotos(bool includeCategories = false)
        {
            SeedCat();

            Seed();

            using (PhotoContext db = new PhotoContext())
            {

                //VECCHIO METODO
                /*if (includeCategories) return db.Photo.Include(c => c.User).Include(i => i.Categories).ToList();

                else return db.Photo.ToList();*/



                if (includeCategories)
                {

                    // Include solo le categorie
                    var photosWithCategories = db.Photo.Include(p => p.Categories).ToList();

                    // Carica separatamente l'attributo UserName per ciascuna foto
                    foreach (var photo in photosWithCategories)
                    {
                        var user = db.Users.FirstOrDefault(u => u.Id == photo.Userid);

                        if (user != null)
                        {
                            //associo alla foto un nuovo oggetto IdentityUser con solo il campo mail
                            photo.User = new IdentityUser { Email = user.Email };
                        }
                    }

                    return photosWithCategories;
                }

                else return db.Photo.ToList();
            }

        }

        public static List<Category> GetAllCategories()
        {
            using (PhotoContext db = new PhotoContext())
            {
                return db.Category.ToList();
            }
        }

        //funzione popolamento tabella categoria foto
        public static void SeedCat()
        {
            using (PhotoContext db = new PhotoContext())
            {
                if (PhotoManager.CountAllPhotos() == 0)
                {
                    var categories = new List<Category>
                    {
                        new Category {Nome = "Still life"},
                        new Category {Nome = "Naturalistica"},
                        new Category {Nome = "Reportage"},
                        new Category {Nome = "Matrimonio"},
                        new Category {Nome = "Glamour"},
                        new Category {Nome = "Viaggio"},
                        new Category {Nome = "Nostalgia"},
                        new Category {Nome = "Svago"}

                    };

                    // Verifica se l'ingrediente esiste già, altrimenti lo aggiunge
                    foreach (var categoria in categories)
                    {

                        db.Category.Add(categoria);
                    }

                    db.SaveChanges();

                }

            }
        }

        //funzione inserimento singole foto
        public static void Seed()
        {
            using (PhotoContext db = new PhotoContext())
            {
                if (PhotoManager.CountAllPhotos() == 0)
                {

                    // Recupera le categorie dal DB
                    var stilllife = db.Category.FirstOrDefault(i => i.Nome == "Still life")?.id.ToString();
                    var naturalistica = db.Category.FirstOrDefault(i => i.Nome == "Naturalistica")?.id.ToString();
                    var reportage = db.Category.FirstOrDefault(i => i.Nome == "Reportage")?.id.ToString();
                    var matrimonio = db.Category.FirstOrDefault(i => i.Nome == "Matrimonio")?.id.ToString();
                    var glamour = db.Category.FirstOrDefault(i => i.Nome == "Glamour")?.id.ToString();
                    var viaggio = db.Category.FirstOrDefault(i => i.Nome == "Viaggio")?.id.ToString();
                    var nostalgia = db.Category.FirstOrDefault(i => i.Nome == "Nostalgia")?.id.ToString();
                    var svago = db.Category.FirstOrDefault(i => i.Nome == "Svago")?.id.ToString();


                    // Crea una lista di stringhe con gli ID delle categorie

                    var primaFoto = new List<String>
                 {
                      stilllife,
                      viaggio,
                      reportage

                 };

                    var secondaFoto = new List<String>
                 {
                     matrimonio,
                     glamour,
                     stilllife
                 };

                    var terzaFoto = new List<String>
                 {
                     naturalistica,
                     reportage,
                     stilllife,
                     glamour
                 };
                    var quartaFoto = new List<String>
                 {
                     nostalgia,
                     svago,
                     glamour
                 };
                    var quintaFoto = new List<String>
                 {
                     nostalgia,
                     svago,
                     stilllife,
                     glamour
                 };



                    PhotoManager.InsertPhoto(new Photo("TourMondiale", "Esperienza fantastica", "~/img/Viaggio1.jpg", true, "2904687b-4d2b-49df-8483-a863c52277f4"), primaFoto);
                    PhotoManager.InsertPhoto(new Photo("Matrimonio", "Bellissimo", "~/img/Matrimonio1.jpg", false, "2904687b-4d2b-49df-8483-a863c52277f4"), secondaFoto);
                    PhotoManager.InsertPhoto(new Photo("TourMondiale2", "Esperienza fantastica2", "~/img/Viaggio2.jpg", true, "2904687b-4d2b-49df-8483-a863c52277f4"), terzaFoto);
                    PhotoManager.InsertPhoto(new Photo("Unreal", "Esperienza Super", "~/img/Unreal.png", false, "2904687b-4d2b-49df-8483-a863c52277f4"), quartaFoto);
                    PhotoManager.InsertPhoto(new Photo("Unreal Tournament", "Esperienza Super al QUADRATO", "~/img/Tournament2.jpg", false, "2904687b-4d2b-49df-8483-a863c52277f4"), quintaFoto);

                    db.SaveChanges();

                }

            }
        }

        //funzione inserimetno effettivo
        public static void InsertPhoto(Photo photo, List<string> SelectedCategories = null)
        {
            using (PhotoContext db = new PhotoContext())
            {
                if (SelectedCategories != null)
                {
                    photo.Categories = new List<Category>();

                    // Trasformiamo gli ID scelti in Ingredient da aggiungere tra i riferimenti in Pizza
                    foreach (var CategoriesId in SelectedCategories)
                    {
                        int id = int.Parse(CategoriesId);
                        var category = db.Category.FirstOrDefault(t => t.id == id); // PostManager.GetTagById(id); NON usiamo GetTagById() perché usa un db context diverso e ciò causerebbe errore in fase di salvataggio - usiamo lo stesso context all'interno della stessa operazione
                        photo.Categories.Add(category);
                    }
                }

                db.Photo.Add(photo);
                db.SaveChanges();
            }

        }

        //aggiornamento effettivo foto
        public static bool UpdatePhoto(int id, Photo photo, List<string> SelectedCategories)
        {

            using PhotoContext db = new PhotoContext();

            //ricerca e restituisce la prima posizione con lo stesso id passato

            var photoDaModificare = db.Photo.Where(p => p.id == id).Include(p => p.Categories).FirstOrDefault();

            if (photoDaModificare == null)
                return false;

            photoDaModificare.Titolo = photo.Titolo;
            photoDaModificare.Descrizione = photo.Descrizione;
            photoDaModificare.ImagePath = photo.ImagePath;
            photoDaModificare.Userid = photoDaModificare.Userid;
            photoDaModificare.ModifydAt = DateTime.Now;

            //verifico prima che sia stato effettivamente selezionato qualcosa
            if (SelectedCategories != null)
            {
                // Poi svuoto così da salvare solo le informazioni che l'utente ha scelto, NON le aggiungiamo ai vecchi dati
                photoDaModificare.Categories.Clear();

                foreach (var category in SelectedCategories)
                {
                    int categoryId = int.Parse(category);
                    var categoryFromDb = db.Category.FirstOrDefault(x => x.id == categoryId);
                    if (categoryFromDb != null)
                        photoDaModificare.Categories.Add(categoryFromDb);
                }
            }

            db.SaveChanges();

            return true;
        }

    }
}
