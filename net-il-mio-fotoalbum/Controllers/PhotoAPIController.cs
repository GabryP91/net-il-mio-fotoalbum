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
        

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromForm] Message message)
        {
            
            // richiamo la funzione per la creazione di un messaggio e ci passo i valori
            MexManager.InsertMessage(message);

            return Ok(new { Messaggio = "Pizza creata con successo" });
        }

    }
}
