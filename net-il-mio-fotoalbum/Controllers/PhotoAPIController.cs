using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoAPIController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAllPhotos()
        {
            var Photos = PhotoManager.GetAllPhotos(true);

            return Ok(Photos);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserMail(string id) // esempio--https://localhost:7077/api/PhotoAPIController/GetUserMail?id=1
        {
            string mail = PhotoManager.GetUserMailById(id);
            if (mail == null)
                return NotFound("ERRORE");
            return Ok(mail);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromForm] Message message)
        {
            
            // richiamo la funzione per la creazione di un messaggio e ci passo i valori
            MexManager.InsertMessage(message);

            return Ok(new { Messaggio = "Messaggio creato con successo" });
        }


        [HttpGet("{title}")] // esempio--https://localhost:7077/api/PhotoAPI/GetPhotoByTitle/Viaggio
        public IActionResult GetPhotoByTitle(string title)
        {
            var Photos = PhotoManager.GetPhotoByTitle(title);
            if (Photos == null)
                return NotFound("ERRORE");
            return Ok(Photos);
        }

    }
}
