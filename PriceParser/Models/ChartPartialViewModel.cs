using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceParser.Models
{
    public class ChartPartialViewModel
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
        public List<SelectListItem> Currencies { get; set; }
    }
}
