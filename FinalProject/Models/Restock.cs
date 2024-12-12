using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Restock
    {
        public int RestockID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public DateTime RestockDate { get; set; }

        [Required]
        public int QuantityAdded { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        // Navigation property
        public virtual Product Product { get; set; }
    }
}