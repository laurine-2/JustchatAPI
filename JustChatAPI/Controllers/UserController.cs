using JustChatAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace JustChatAPI.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("users")]
        public object AddUser([FromBody] User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return Ok("User Add");
        }
    }
}
