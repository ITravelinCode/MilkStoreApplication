using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.PaymentView
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public double PaymentAmount { get; set; }
        public int AccountId { get; set; }
    }
}
