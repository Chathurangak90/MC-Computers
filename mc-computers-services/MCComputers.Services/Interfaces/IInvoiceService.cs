using MCComputers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCComputers.Services.Interfaces
{
 
    public interface IInvoiceService
    {
        // Creates a new invoice
        Task<InvoiceResponseModel> CreateInvoiceAsync(InvoiceCreateModel invoiceModel);

        // Retrieves an invoice by its ID 
        Task<InvoiceResponseModel> GetInvoiceByIdAsync(int invoiceId);

        // Retrieves all invoices for a given customer
        Task<IEnumerable<InvoiceResponseModel>> GetCustomerInvoicesAsync(int customerId);

        // Retrieves all customers
        Task<IEnumerable<CustomerModel>> GetAllCustomersAsync();

        // Retrieves all products
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
    }
}
