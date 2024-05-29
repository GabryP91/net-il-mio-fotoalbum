using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
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

                model.CreateCategories();

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

                // Verifica se uno degli errori riguarda il campo "foto"

                if (errorMessages.Count != 1 || foto == null || foto.Length == 0)
                {
                    //data.Categories = PhotoManager.GetAllCategories();

                    data.CreateCategories();

                    return View("Update", data);
                }
            }

            // Ottieni il nome del file immagine caricato dall'utente
            string imgFileName = Path.GetFileName(foto.FileName);

            string imgFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            string imgPath = Path.Combine(imgFolderPath, imgFileName);

            data.Photo.ImagePath = "~/img/" + imgFileName;

            // richiamo funzione di modifica
            bool result = PhotoManager.UpdatePhoto(id, data.Photo, data.SelectedCategories);

            if (result == true)
                return RedirectToAction("Index");
            else
                return NotFound();

        }
    }
}
