using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AytoLosLLanos.Models
{
    public class Item1
    {
        [Key]
        public DateTime Fecha { get; set; }
        public List<Valores> valores { get; set; }
        public List<Nombres> nombres { get; set; }
    }

    public class Valores
    {
        public double valor { get; set; }
    }

    public class Nombres
    {
        public string nombre { get; set; }
    }
}