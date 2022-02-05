﻿using Microsoft.AspNetCore.Identity;

namespace PriceParcer.Models
{
    public class MarketSiteDetailsViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? AuthType { get; set; }
        public string? SiteLogin { get; set; }
        public string? SitePassword { get; set; }
        public string? ParseType { get; set; }

        public bool IsAvailable { get; set; }
        public DateTime Created { get; set; }
        public IdentityUser? CreatedByUser { get; set; }
        
    }
}