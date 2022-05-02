﻿using Microsoft.AspNetCore.Identity;
using PriceParser.Data;
using PriceParser.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.DTO
{
    public class UserReviewDTO
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
        public ProductDTO Product { get; set; }
        public Guid ProductId { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Hidden { get; set; }
    }
}
