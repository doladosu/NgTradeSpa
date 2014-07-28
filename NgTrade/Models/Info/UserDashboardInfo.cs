using System;
using NgTrade.Models.Data;

namespace NgTrade.Models.Info
{
    public class UserDashboardInfo
    {
        public UserProfile UserProfile { get; set; }
        public string DisplayName { get; set; }
        public Decimal Balance { get; set; }

    }
}