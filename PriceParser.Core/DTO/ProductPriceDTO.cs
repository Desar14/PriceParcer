using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.DTO
{
    public class ProductPriceDTO
    {
        public Guid Id { get; set; }

        public Guid ProductFromSiteId { get; set; }

        public DateTime ParseDate { get; set; }
        public double FullPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountPercent { get; set; }
        public string CurrencyCode { get; set; }
        public Guid CurrencyId { get; set; }
        public bool IsOutOfStock { get; set; }
        public bool ParseError { get; set; }
    }
}
