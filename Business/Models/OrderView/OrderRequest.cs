using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.OrderView
{
    public class OrderRequest
    {
        public double TotalPrice { get; set; }
        public int PaymentId { get; set; }
        public List<OrderDetailRequest>? OrderDetails { get; set; }
    }
}
