using PriceParser.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.Utils
{
    static public class PricesProcessing
    {
        public static IEnumerable<ProductPriceDTO> MakePricesPerEveryDay(IEnumerable<ProductPriceDTO> productPrices, DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;

            List<DateTime> dates = new List<DateTime>();
            for (DateTime i = startDate; i <= endDate; i = i.AddDays(1))
            {
                dates.Add(i);
            }

            //just an awful code, but looks like group by needs anonymous types, and automapper can't work with them
            var groupedPricesInDay = productPrices
                .GroupBy(
                g => new
                {
                    g.ProductFromSiteId,
                    ParseDate = g.ParseDate.Date,
                    g.DiscountPrice,
                    g.DiscountPercent,
                    g.CurrencyCode,
                    g.CurrencyId,
                    g.IsOutOfStock,
                    g.ParseError
                },
                c => c.FullPrice,
                (g, c) => new ProductPriceDTO()
                {
                    Id = Guid.Empty,
                    ProductFromSiteId = g.ProductFromSiteId,
                    ParseDate = g.ParseDate,
                    FullPrice = c.Average(),
                    DiscountPrice = g.DiscountPrice,
                    DiscountPercent = g.DiscountPercent,
                    CurrencyCode = g.CurrencyCode,
                    CurrencyId = g.CurrencyId,
                    IsOutOfStock = g.IsOutOfStock,
                    ParseError = g.ParseError
                }
                );

            //var groupedPricesInDay1 = productPrices
            //    .GroupBy(
            //    g => _mapper.Map<ProductPriceDTO>(g, opt => 
            //        opt.AfterMap((src, dest) => {
            //            dest.ParseDate = dest.ParseDate.Date;
            //            dest.Id = Guid.Empty;
            //        })              
            //        ),
            //    c => c.FullPrice,
            //    (g, c) => _mapper.Map<ProductPriceDTO>(g, opt => opt.AfterMap((src, dest) => dest.FullPrice = c.Average())));

            var datesForJoining = groupedPricesInDay
                .Join(dates, t1 => 1, t2 => 1, (t1, t2) => new
                {
                    t1.ProductFromSiteId,
                    t1.ParseDate,
                    ResultDate = t2
                })
                .Where(x => x.ResultDate >= x.ParseDate)
                .GroupBy(g => new { g.ProductFromSiteId, g.ResultDate }, c => c.ParseDate,
                (g, c) => new
                {
                    g.ProductFromSiteId,
                    g.ResultDate,
                    ParseDate = c.Max()
                })
                .OrderBy(x => x.ResultDate);

            var result = datesForJoining.Join(groupedPricesInDay,
                t1 => new { t1.ProductFromSiteId, t1.ParseDate },
                t2 => new { t2.ProductFromSiteId, t2.ParseDate },
                (t1, t2) => new ProductPriceDTO()
                {
                    Id = Guid.Empty,
                    ProductFromSiteId = t2.ProductFromSiteId,
                    ParseDate = t1.ResultDate,
                    FullPrice = t2.FullPrice,
                    DiscountPrice = t2.DiscountPrice,
                    DiscountPercent = t2.DiscountPercent,
                    CurrencyCode = t2.CurrencyCode,
                    CurrencyId = t2.CurrencyId,
                    IsOutOfStock = t2.IsOutOfStock,
                    ParseError = t2.ParseError
                });

            return result;
        }
    }
}
