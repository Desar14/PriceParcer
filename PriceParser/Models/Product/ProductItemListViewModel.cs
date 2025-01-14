﻿namespace PriceParser.Models
{
    public class ProductItemListViewModel
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double BestPriceNow { get; set; }
        public float AverageScore { get; set; }
        public string? CurrencyCode { get; set; }

        public string? ImagePath { get; set; }

        public ProductItemListViewModel()
        {
            Name = "";
            Category = "";
        }
    }
}
