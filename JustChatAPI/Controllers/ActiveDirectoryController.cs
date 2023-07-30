using JustChatAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JustChatAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActiveDirectoryController : ControllerBase
    {
        private readonly ActiveDirectoryService _adService;

        public ActiveDirectoryController(ActiveDirectoryService adService)
        {
            _adService = adService;
        }

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] Models.User user )
        {
            _adService.CreateUser(user.Username, user.Password);
            return Ok("utilisateur crée avec success");

        }

        [HttpPost("connect")]
        public IActionResult AuthenficateUser([FromBody] Models.User user )
        {
            bool isAuthenficated=_adService.AuthentificateUser(user.Username, user.Password);
            if (isAuthenficated)
            {
                return Ok("Authentification Successfull completed");
            }
            else
            {
                return Unauthorized("Authentification Not Completed Yet");
            }
        }

        [HttpGet("contacts")]
        public IActionResult getContacts()
        {
            List<string> contacts= _adService.getContacts();
            if(contacts.Count==0) 
            {
                return NotFound("Aucun contact pour cette unite organisationelle");
            }
            else
            {
                return Ok(contacts);
            }
        }

    }
}
