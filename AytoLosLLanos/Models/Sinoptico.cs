using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AytoLosLLanos.Models
{
    public partial class Sinoptico
    {
        public DateTime Fecha { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public string Color { get; set; }
        public string Color2 { get; set; }
    }

    public partial class Sinoptico
    {
        public List<Sinoptico> itemModel { get; set; }
    }
}