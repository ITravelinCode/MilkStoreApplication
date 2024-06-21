using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(10)]
        public string? Phone { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }

        public DateTime? Dob { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int Status { get; set; }
        public ICollection<Order>? orders { get; set; }
        public ICollection<Cart>? carts { get; set; }
        public ICollection<Payment>? payments { get; set; }
    }
}
