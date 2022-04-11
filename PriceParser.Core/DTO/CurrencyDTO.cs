using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceParser.Core.DTO
{
    public class CurrencyDTO
    {
        public Guid Id { get; set; }
        public int Cur_ID { get; set; }
        public int? Cur_ParentID { get; set; }
        public string Cur_Code { get; set; }
        public string Cur_Abbreviation { get; set; }
        public string Cur_Name { get; set; }
        public int Cur_Scale { get; set; }
        public int Cur_Periodicity { get; set; }

        public bool UpdateRates { get; set; }
        public bool AvailableForUsers { get; set; }
    }
}
