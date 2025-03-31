using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCComputers.Models
{
    public class InvoiceCreateModel
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<InvoiceItemModel> Items { get; set; }

        public decimal? DiscountAmount { get; set; }
    }
}
