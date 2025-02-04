﻿using PriceParser.Data.Entities;

namespace PriceParser.Core.DTO
{
    public class ProductFromSitesDTO
    {
        public Guid Id { get; set; }

        public ProductDTO Product { get; set; }
        public Guid ProductId { get; set; }

        public MarketSiteDTO Site { get; set; }
        public Guid SiteId { get; set; }

        public string? Path { get; set; }
        public bool DoNotParse { get; set; }

        public string? ParseSchedule { get; set; }

        public DateTime Created { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
        public string? CreatedByUserId { get; set; }

        public List<ProductPriceDTO> Prices { get; set; } = new();
    }
}
