using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{

    public class PhotoController : Controller
    {

        //acquisisco l'id dello user
        private readonly UserManager<IdentityUser> _userManager;

        public PhotoController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(PhotoManager.GetAllPhotos(true));
        }

        public IActionResult Details(int id)
        {
            //restituiscimi la foto con id uguale a quello passato
            var photo = PhotoManager.GetPhotoById(id);

            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PhotoFormModel model = new PhotoFormModel();
            model.Photo = new Photo();

            // Get the currently logged-in user's ID
            string userId = _userManager.GetUserId(User);
            model.Photo.Userid = userId;

            model.CreateCategories(PhotoManager.GetAllCategories());

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(PhotoFormModel data, IFormFile foto)
        {

            if (!ModelState.IsValid)
            {

                // Ottenere la lista degli errori di validazione
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // Verifica se ci sono errori o se la foto non è presente
                if (errorMessages.Count > 1 || foto == null || foto.Length == 0)
                {

                    data.CreateCategories(PhotoManager.GetAllCategories());

                    return View("Create", data);
                }

            }

            string imgFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
            string imgPath = Path.Combine(imgFolderPath, imgFileName);

            using (var stream = new FileStream(imgPath, FileMode.Create))
            {
                await foto.CopyToAsync(stream);
            }
          
            data.Photo.ImagePath = "~/img/" + imgFileName;

            PhotoManager.InsertPhoto(data.Photo, data.SelectedCategories);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            // Prendo il post AGGIORNATO da database, non
            // uno passato da utente alla action
            var photoToEdit = PhotoManager.GetPhotoById(id);

            if (photoToEdit == null)
            {
                return NotFound();
            }

            else
            {
                PhotoFormModel model = new PhotoFormModel(photoToEdit);

                model.CreateCategories(PhotoManager.GetAllCategories());

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PhotoFormModel data, IFormFile foto)
        {
            if (!ModelState.IsValid)
            {

                // ottengo la lista degli errori
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
 
                bool hasTitleError = errorMessages.Any(e => e.Contains("Il titolo è obbligatorio"));

                bool hasDescriptionError = errorMessages.Any(e => e.Contains("La descrizione è obbligatoria"));

                // Verifica se uno degli errori riguarda il campo "foto"

                if (hasTitleError || hasDescriptionError)
                {
                  
                    data.CreateCategories(PhotoManager.GetAllCategories());

                    return View("Update", data);
                }
            }

            string imgFileName;

            if (foto == null)
            {
                // se l'immagine non è stata caricata inserisci una stringa vuota
                
                data.Photo.ImagePath = "";
            }

            else
            {
                // Ottieni il nome del file immagine caricato dall'utente
                imgFileName = Path.GetFileName(foto.FileName);
                data.Photo.ImagePath = "~/img/" + imgFileName;
            }



            

            // richiamo funzione di modifica
            bool result = PhotoManager.UpdatePhoto(id, data.Photo, data.SelectedCategories);

            if (result == true)
                return RedirectToAction("Index");
            else
                return NotFound();

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {

            if (PhotoManager.DeletePhoto(id))
                return RedirectToAction("Index");
            else
                return NotFound();
        }
    }
}
