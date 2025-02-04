﻿namespace PriceParser.Data.Entities
{
    public class ProductPrice : BaseEntity
    {
        public virtual ProductFromSites ProductFromSite { get; set; }
        public Guid ProductFromSiteId { get; set; }

        public DateTime ParseDate { get; set; }
        public double FullPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountPercent { get; set; }
        public string CurrencyCode { get; set; }
        public virtual Currency Currency { get; set; }
        public Guid CurrencyId { get; set; }
        public bool IsOutOfStock { get; set; }
        public bool ParseError { get; set; }


    }
}
