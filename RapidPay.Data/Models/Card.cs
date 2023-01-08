using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RapidPay.Data.Models
{
    public class Card
    {
        [Key]
        public long Number { get; set; }
        public double Balance { get; set; } = 0;
    }
}
