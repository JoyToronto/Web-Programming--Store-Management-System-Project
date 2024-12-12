using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public DateTime OrderDate { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; } // Navigation property

        public decimal TotalAmount
        {
            get
            {
                return OrderProducts?.Sum(op => op.Quantity * op.Product.Price) ?? 0;
            }
        }
    }

    public class OrderItem
    {
        public int OrderItemID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}