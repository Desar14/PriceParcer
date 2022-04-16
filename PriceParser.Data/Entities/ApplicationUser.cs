using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual Currency? UserCurrency { get; set; }
        public Guid? UserCurrencyId { get; set; }
    }
}
