﻿using Microsoft.AspNetCore.Identity;
using PriceParcer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.Core.DTO
{
    public class MarketSiteDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? AuthType { get; set; }
        public string? SiteLogin { get; set; }
        public string? SitePassword { get; set; }
        public ParseTypes? ParseType { get; set; }
        public string? ParsePricePath { get; set; }
        public string? ParseCurrencyPath { get; set; }
        public string? ParsePriceAttributeName { get; set; }
        public string? ParseCurrencyAttributeName { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime Created { get; set; }
        public IdentityUser? CreatedByUser { get; set; }

        public string? CreatedByUserId { get; set; }

    }
}
