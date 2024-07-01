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
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ExpireDate { get; set; }
        [Required]
        public double PaymentAmount { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account account { get; set; }

    }
}
