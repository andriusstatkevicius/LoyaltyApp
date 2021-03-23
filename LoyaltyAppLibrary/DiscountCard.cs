using System;
using System.Collections.Generic;

namespace LoyaltyAppLibrary
{
    public class DiscountCard
    {
        public int CardNumber { get; set; }
        public Client Client { get; set; }
        public DateTime IssuingDate { get; set; }
        public DateTime ValidUntilDate { get; set; }
        public List<LoyaltyTransaction> LoyaltyTransactions { get; set; }
    }
}
