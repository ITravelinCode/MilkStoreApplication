using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.OrderView
{
    public class OrderRequest
    {
        public int OrderId { get; set; }    
        public List<OrderDetailRequest>? OrderDetails { get; set; }
    }
}
