using DataAccess.Entities;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.OrderView
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public double TotalPrice { get; set; }
        public int Status { get; set; }
        public ICollection<OrderDetailResponse>? orderDetails { get; set; }
    }
}
