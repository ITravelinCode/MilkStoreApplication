using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        [Required]
        [StringLength(100)]
        public string? PaymentMethod { get; set; }
        [Required]
        public string? BankCode { get; set; }
        [Required]
        public string? BankTranNo { get; set; }
        [Required]
        public string? CardType { get; set; }
        public string? PaymentInfo { get; set; }

        public DateTime PayDate { get; set; }
        [Required]
        public string? TransactionNo { get; set; }
        [Required]
        public int TransactionStatus { get; set; }
        [Required]
        public double PaymentAmount { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
    }
}
