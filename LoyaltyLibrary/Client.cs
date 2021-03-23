using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoyaltyLibrary
{
    public class Client
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Surname { get; set; }
        [Required] public string Email { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfRegistration { get; set; } = DateTime.UtcNow;
        public List<DiscountCard> DiscountCards { get; set; } = new List<DiscountCard>();
    }
}
