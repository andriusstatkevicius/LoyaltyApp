using System;

namespace LoyaltyAppLibrary
{
    public class LoyaltyTransaction
    {
        public int CardNumber { get; set; }
        public int ClientId { get; set; }
        public int LoyaltyPointsAmount { get; set; }
        public DateTime TransactionTime { get; set; }
    }
}
