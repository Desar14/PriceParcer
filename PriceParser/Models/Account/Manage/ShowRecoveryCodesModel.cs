using Microsoft.AspNetCore.Mvc;

namespace PriceParser.Models.Account.Manage
{
    public class ShowRecoveryCodesModel
    {
        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
    }
}
