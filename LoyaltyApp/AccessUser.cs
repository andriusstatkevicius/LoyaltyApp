using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoyaltyApp
{
    public class AccessUser : IdentityUser
    {
        [NotMapped]public string Password { get; set; }
        [NotMapped]public bool IsAbleToIssue { get; set; }
        [NotMapped]public bool IsAbleToRecord { get; set; }
    }
}
