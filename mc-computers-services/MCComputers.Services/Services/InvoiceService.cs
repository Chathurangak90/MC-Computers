using AutoMapper;
using MCComputers.Models;
using MCComputers.Repositories.Interfaces;
using MCComputers.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCComputers.Services.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;
        private readonly IMapper _mapper;

        // Constructor to initialize repository and mapper
        public InvoiceService(IInvoiceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Method to create a new invoice
        public async Task<InvoiceResponseModel> CreateInvoiceAsync(InvoiceCreateModel invoiceCreateModel)
        {
            return await _repository.CreateInvoiceAsync(invoiceCreateModel);
        }

        // Method to get all customers
        public async Task<IEnumerable<CustomerModel>> GetAllCustomersAsync()
        {
            return await _repository.GetAllCustomersAsync();
        }

        // Method to get all available products
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await _repository.GetAllProductsAsync();
        }

        // Method to get all invoices for a specific customer
        public async Task<IEnumerable<InvoiceResponseModel>> GetCustomerInvoicesAsync(int customerId)
        {
            return await _repository.GetCustomerInvoicesAsync(customerId);
        }

        // Method to get a specific invoice by its ID
        public async Task<InvoiceResponseModel> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _repository.GetInvoiceByIdAsync(invoiceId);
        }
    }
}
