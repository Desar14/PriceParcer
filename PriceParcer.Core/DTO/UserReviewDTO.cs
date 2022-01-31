using Microsoft.AspNetCore.Identity;
using PriceParcer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.Core.DTO
{
    public class UserReviewDTO
    {
        public Guid Id { get; set; }
        public IdentityUser User { get; set; }        
        public Product Product { get; set; }   
        public string ReviewText { get; set; }
        public int ReviewScore { get; set; }
        public bool Hidden { get; set; }
    }
}
