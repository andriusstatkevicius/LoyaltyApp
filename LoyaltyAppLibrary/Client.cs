using System;
using System.Collections.Generic;

namespace LoyaltyAppLibrary
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public List<DiscountCard> DiscountCards { get; } = new List<DiscountCard>();
    }
}
