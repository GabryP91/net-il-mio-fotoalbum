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


    }
}
