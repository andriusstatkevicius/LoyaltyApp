using System.ComponentModel.DataAnnotations;

namespace LoyaltyApp
{
    public class Role
    {
        [Required] public string Name { get; set; }
    }
}
