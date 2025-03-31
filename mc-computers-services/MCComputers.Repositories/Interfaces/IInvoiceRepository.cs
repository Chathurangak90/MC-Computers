using MCComputers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCComputers.Repositories.Interfaces
{
    // Interface for invoice-related data operations
    public interface IInvoiceRepository
    {
        // Method to create a new invoice
        Task<InvoiceResponseModel> CreateInvoiceAsync(InvoiceCreateModel invoiceModel);

        // Method to retrieve a specific invoice by its ID
        Task<InvoiceResponseModel> GetInvoiceByIdAsync(int invoiceId);

        // Method to retrieve all invoices for a specific customer
        Task<IEnumerable<InvoiceResponseModel>> GetCustomerInvoicesAsync(int customerId);

        // Method to retrieve all customers
        Task<IEnumerable<CustomerModel>> GetAllCustomersAsync();

        // Method to retrieve all available products
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
    }
}
