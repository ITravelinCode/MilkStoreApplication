using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public int Status { get; set; } //0 = Pending, 1 = Paid, 2 = Failed
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
        [ForeignKey("AccountId")]
        public Account account { get; set; }
        public ICollection<OrderDetail>? orderDetails { get; set; }
    }
}
