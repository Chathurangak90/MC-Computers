using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCComputers.Models
{
    public  class InvoiceModel
    {
        public DateTime TransactionDate { get; set; }
        public List<InvoiceItemModel> Items { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BalanceAmount { get; set; }

    }
}
