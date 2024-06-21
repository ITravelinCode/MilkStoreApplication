using DataAccess.Entities;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.CartView
{
    public class CartResponse
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int AccountId { get; set; }
        public int CartQuantity { get; set; }
        public int Status { get; set; }
        public Product Product { get; set; }
        public Account Account { get; set; }
    }
}
