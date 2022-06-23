using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OpenIdConnectPoC.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IdentityDbContext _dbContext;

        public ValuesController(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("users")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await this._dbContext.Users.Select(u => new UserDto(u.UserName)).ToListAsync();

            return Ok(users);
        }
        
        [HttpGet("my-profile")]
        [Authorize]
        public ActionResult<UserDto> MyProfile()
        {
            var claim = this.User?.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

            return new UserDto(claim.Value);
        }
    }

    public record UserDto(string UserName);

}
