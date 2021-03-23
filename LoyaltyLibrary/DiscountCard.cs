using System;
using System.Collections.Generic;

namespace LoyaltyLibrary
{
    public class DiscountCard
    {
        public int Number { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public DateTime IssuingDate { get; set; } = DateTime.UtcNow;
        public DateTime ValidUntilDate { get; set; }
        public List<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new List<LoyaltyTransaction>();

        public DiscountCard()
        {
            var issuingDate = new DateTime(IssuingDate.Year, IssuingDate.Month, 1);
            ValidUntilDate = issuingDate.AddMonths(1).AddDays(-1).AddYears(3); // Adding 3 years for expiry and the last day of the month
        }
    }
}
