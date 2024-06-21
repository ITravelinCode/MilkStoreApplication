using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Auth
{
    public class UpdateAccountRequest : RegisterRequest
    {
        public int Status { get; set; }
    }
}
