using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class CartItemGroupByModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }


        public double TotalPrice { get; set; }
        public int ProductCount { get; set; }
    }
}
