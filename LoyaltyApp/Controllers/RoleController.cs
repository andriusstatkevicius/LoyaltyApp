using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoyaltyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            var identityRole = new IdentityRole(role.Name);
            await _roleManager.CreateAsync(identityRole);

            return CreatedAtAction("CreateRole", new { id = identityRole.Id }, role);
        }
    }
}
