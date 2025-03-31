using MCComputers.API.Controllers;
using MCComputers.Models;
using MCComputers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCComputers.UnitTest
{
    public class InvoiceControllerTest
    {
        private readonly Mock<IInvoiceService> _mockInvoiceService;
        private readonly Mock<ILogger<InvoiceController>> _mockLogger;
        private readonly InvoiceController _controller;

        public InvoiceControllerTest()
        {
            _mockInvoiceService = new Mock<IInvoiceService>();
            _mockLogger = new Mock<ILogger<InvoiceController>>();
            _controller = new InvoiceController(_mockInvoiceService.Object, _mockLogger.Object);
        }

        // Test: Create an invoice successfully
        [Fact]
        public async Task CreateInvoice_ShouldReturnCreatedInvoice()
        {
            // Arrange
            var invoiceModel = new InvoiceCreateModel { /* set properties */ };
            var createdInvoice = new InvoiceResponseModel { Id = 1, /* set properties */ };
            _mockInvoiceService.Setup(x => x.CreateInvoiceAsync(It.IsAny<InvoiceCreateModel>())).ReturnsAsync(createdInvoice);

            // Act
            var result = await _controller.CreateInvoice(invoiceModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InvoiceResponseModel>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(createdInvoice, createdAtActionResult.Value);
        }

        // Test: Get invoice by ID successfully
        [Fact]
        public async Task GetInvoiceById_ShouldReturnInvoice()
        {
            // Arrange
            var invoiceId = 1;
            var invoice = new InvoiceResponseModel { Id = invoiceId, /* set properties */ };
            _mockInvoiceService.Setup(x => x.GetInvoiceByIdAsync(invoiceId)).ReturnsAsync(invoice);

            // Act
            var result = await _controller.GetInvoiceById(invoiceId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InvoiceResponseModel>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(invoice, okResult.Value);
        }

        // Test: Get invoice by ID not found
        [Fact]
        public async Task GetInvoiceById_ShouldReturnNotFound()
        {
            // Arrange
            var invoiceId = 1;
            _mockInvoiceService.Setup(x => x.GetInvoiceByIdAsync(invoiceId)).ThrowsAsync(new ArgumentException());

            // Act
            var result = await _controller.GetInvoiceById(invoiceId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InvoiceResponseModel>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        // Test: Get invoices by customer ID
        [Fact]
        public async Task GetCustomerInvoices_ShouldReturnInvoices()
        {
            // Arrange
            var customerId = 1;
            var invoices = new List<InvoiceResponseModel> { new InvoiceResponseModel { Id = 1 } };
            _mockInvoiceService.Setup(x => x.GetCustomerInvoicesAsync(customerId)).ReturnsAsync(invoices);

            // Act
            var result = await _controller.GetCustomerInvoices(customerId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<InvoiceResponseModel>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(invoices, okResult.Value);
        }

        // Test: Get all customers
        [Fact]
        public async Task GetCustomers_ShouldReturnCustomers()
        {
            // Arrange
            var customers = new List<CustomerModel> { new CustomerModel { /* set properties */ } };
            _mockInvoiceService.Setup(x => x.GetAllCustomersAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CustomerModel>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(customers, okResult.Value);
        }

        // Test: Get all products
        [Fact]
        public async Task GetProducts_ShouldReturnProducts()
        {
            // Arrange
            var products = new List<ProductModel> { new ProductModel { /* set properties */ } };
            _mockInvoiceService.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductModel>>>(result); // Expecting ProductModel now
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(products, okResult.Value);
        }

    }
}
