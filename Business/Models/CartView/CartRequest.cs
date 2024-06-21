using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.CartView
{
    public class CartRequest
    {
        public int ProductId { get; set; }
        public int AccountId { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
    }
}
