using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCComputers.Models
{
    public class InvoiceResponseModel
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public List<InvoiceItemResponseModel> Items { get; set; }
    }
}
