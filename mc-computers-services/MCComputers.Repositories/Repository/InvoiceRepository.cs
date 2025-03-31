using AutoMapper;
using MCComputers.Entities;
using MCComputers.Models;
using MCComputers.Repositories.DBContext;
using MCComputers.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCComputers.Repositories.Repository
{
    // Repository for managing invoice-related database operations
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceRepository> _logger;

        public InvoiceRepository(AppDbContext repository, IMapper mapper, ILogger<InvoiceRepository> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        // Creates a new invoice asynchronously with transaction handling
        public async Task<InvoiceResponseModel> CreateInvoiceAsync(InvoiceCreateModel invoiceCreateModel)
        {
            using var transaction = await _repository.Database.BeginTransactionAsync();
            try
            {
                // Validate customer existence
                var customer = await _repository.Customers.FirstOrDefaultAsync(c => c.Id == invoiceCreateModel.CustomerId);
                if (customer == null)
                    throw new ArgumentException("Invalid customer.");

                // Validate products and calculate total amount
                var invoiceItems = new List<InvoiceItem>();
                decimal totalAmount = 0;

                foreach (var item in invoiceCreateModel.Items)
                {
                    var product = await _repository.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    if (product == null)
                        throw new ArgumentException($"Product {item.ProductId} not found.");
                    if (product.StockQuantity < item.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for product {product.Name}.");

                    // Update stock and calculate line total
                    product.StockQuantity -= item.Quantity;
                    var lineTotal = product.Price * item.Quantity;
                    totalAmount += lineTotal;

                    invoiceItems.Add(new InvoiceItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        LineTotal = lineTotal
                    });
                }

                // Apply discount and calculate final amount
                decimal discountPercentage = invoiceCreateModel.DiscountAmount ?? 0;
                decimal discountAmount = (discountPercentage / 100) * totalAmount;
                decimal balanceAmount = totalAmount - discountAmount;

                var invoice = new Invoice
                {
                    CustomerId = invoiceCreateModel.CustomerId,
                    TransactionDate = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    DiscountAmount = discountAmount,
                    BalanceAmount = balanceAmount,
                    InvoiceItems = invoiceItems
                };

                _repository.Invoices.Add(invoice);
                await _repository.SaveChangesAsync();
                await transaction.CommitAsync();

                // Map to response model
                return new InvoiceResponseModel
                {
                    Id = invoice.Id,
                    TransactionDate = invoice.TransactionDate,
                    TotalAmount = invoice.TotalAmount,
                    DiscountAmount = invoice.DiscountAmount,
                    BalanceAmount = invoice.BalanceAmount,
                    Items = invoice.InvoiceItems.Select(ii => new InvoiceItemResponseModel
                    {
                        ProductId = ii.ProductId,
                        ProductName = _repository.Products.Find(ii.ProductId).Name,
                        Quantity = ii.Quantity,
                        UnitPrice = ii.UnitPrice,
                        LineTotal = ii.LineTotal
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating invoice");
                throw;
            }
        }

        // Retrieves all customers asynchronously
        public async Task<IEnumerable<CustomerModel>> GetAllCustomersAsync()
        {
            return await _repository.Customers.Select(c => new CustomerModel
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber
            }).ToListAsync();
        }

        // Retrieves all products asynchronously
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await _repository.Products.Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                StockQuantity = p.StockQuantity,
                Price = p.Price
            }).ToListAsync();
        }

        // Retrieves all invoices for a given customer asynchronously
        public async Task<IEnumerable<InvoiceResponseModel>> GetCustomerInvoicesAsync(int customerId)
        {
            var invoices = await _repository.Invoices
                .Where(i => i.CustomerId == customerId)
                .Include(i => i.InvoiceItems)
                .ToListAsync();

            var productIds = invoices.SelectMany(i => i.InvoiceItems)
                                     .Select(ii => ii.ProductId)
                                     .Distinct()
                                     .ToList();

            var productNames = await _repository.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.Name);

            return invoices.Select(invoice => new InvoiceResponseModel
            {
                Id = invoice.Id,
                TransactionDate = invoice.TransactionDate,
                TotalAmount = invoice.TotalAmount,
                DiscountAmount = invoice.DiscountAmount,
                BalanceAmount = invoice.BalanceAmount,
                Items = invoice.InvoiceItems.Select(ii => new InvoiceItemResponseModel
                {
                    ProductId = ii.ProductId,
                    ProductName = productNames.ContainsKey(ii.ProductId) ? productNames[ii.ProductId] : "Unknown Product",
                    Quantity = ii.Quantity,
                    UnitPrice = ii.UnitPrice,
                    LineTotal = ii.LineTotal
                }).ToList()
            });
        }

        // Retrieves an invoice by its ID asynchronously
        public async Task<InvoiceResponseModel> GetInvoiceByIdAsync(int invoiceId)
        {
            var invoice = await _repository.Invoices
                            .Include(i => i.InvoiceItems)
                            .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                throw new ArgumentException("Invoice not found.");

            return new InvoiceResponseModel
            {
                Id = invoice.Id,
                TransactionDate = invoice.TransactionDate,
                TotalAmount = invoice.TotalAmount,
                DiscountAmount = invoice.DiscountAmount,
                BalanceAmount = invoice.BalanceAmount,
                Items = invoice.InvoiceItems.Select(ii => new InvoiceItemResponseModel
                {
                    ProductId = ii.ProductId,
                    ProductName = _repository.Products.Find(ii.ProductId).Name,
                    Quantity = ii.Quantity,
                    UnitPrice = ii.UnitPrice,
                    LineTotal = ii.LineTotal
                }).ToList()
            };
        }
    }
}