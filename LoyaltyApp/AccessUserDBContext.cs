using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyApp
{
    public class AccessUserDBContext : IdentityDbContext<AccessUser>
    {
        public AccessUserDBContext(DbContextOptions<AccessUserDBContext> options) : base(options)
        {
        }
    }
}
