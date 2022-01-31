using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.Core.DTO
{
    public class ProductFromSitesDTO
    {
        public Guid Id { get; set; }

        public ProductDTO Product { get; set; }       

        public  MarketSiteDTO Site { get; set; }
        
        public string? Path { get; set; }
        public bool DoNotParse { get; set; }

        public string? ParseSchedule { get; set; }

        public DateTime Created { get; set; }
        public IdentityUser? CreatedByUser { get; set; }       

        //public virtual List<ProductPrice> Prices { get; set; }

    }
}
