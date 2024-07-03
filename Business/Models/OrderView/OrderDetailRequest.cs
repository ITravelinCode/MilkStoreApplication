using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.OrderView
{
    public class OrderDetailRequest
    {
        public int ProductId { get; set; }
        public int OrderQuantity { get; set; }
        public double ProductPrice { get; set; }
    }
}
