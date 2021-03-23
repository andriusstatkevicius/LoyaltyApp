using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoyaltyLibrary
{
    public class LoyaltyTransaction
    {
        public int Id { get; set; }

        public int CardNumber { get; set; }
        public DiscountCard Card { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal LoyaltyPointsAmount { get; set; }
        public DateTime TransactionTime { get; set; } = DateTime.UtcNow;
    }
}
