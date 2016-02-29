using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AytoLosLLanos.Models
{
    public class Item2
    {
        [Key]
        public DateTime Fecha { get; set; }
        public double Consigna1 { get; set; }
        public double Caudal1 { get; set; }
        public double Acumulado1 { get; set; }
        public double Posicion1 { get; set; }
    }
}