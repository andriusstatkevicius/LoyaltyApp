using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoyaltyApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<AccessUser> _userManager;

        public HomeController(UserManager<AccessUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(AccessUser accessUser)
        {
            var user = await _userManager.FindByNameAsync(accessUser.UserName);

            IdentityResult result = user is null ? await _userManager.CreateAsync(accessUser, accessUser.Password) : null;

            if (result?.Succeeded == true)
            {
                // TODO: Getting a nasty error here (could not figure out what's  the issue yet)
                if (accessUser.IsAbleToIssue)
                    await _userManager.AddToRoleAsync(accessUser, "Issuer");

                if (accessUser.IsAbleToRecord)
                    await _userManager.AddToRoleAsync(accessUser, "Recorder");

                return Ok();
            }

            return BadRequest(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AccessUser accessUser)
        {
            var user = await _userManager.FindByNameAsync(accessUser.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, accessUser.Password))
            {
                var identity = new ClaimsIdentity("cookies");
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));
                return Ok("Login successful");
            }

            return BadRequest();
        }
    }
}
