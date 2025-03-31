using MCComputers.Models;
using MCComputers.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCComputers.API.Controllers
{
    [Route("api/invoice")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService invoiceService, ILogger<InvoiceController> logger)
        {
            _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Endpoint to create an invoice
        [HttpPost("create")]
        public async Task<ActionResult<InvoiceResponseModel>> CreateInvoice([FromBody] InvoiceCreateModel invoiceModel)
        {
            try
            {
                var createdInvoice = await _invoiceService.CreateInvoiceAsync(invoiceModel);
                return CreatedAtAction(
                    nameof(GetInvoiceById), // Reference to the GetInvoiceById method
                    new { id = createdInvoice.Id },
                    createdInvoice);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid invoice creation request");
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Business logic violation");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during invoice creation");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" });
            }
        }

        // Endpoint to retrieve all invoices for a specifi invoice id
        [HttpGet("getinvoicebyid")]
        public async Task<ActionResult<InvoiceResponseModel>> GetInvoiceById(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
                return Ok(invoice);
            }
            catch (ArgumentException)
            {
                return NotFound(new { message = "Invoice not found" });
            }
        }

        // Endpoint to retrieve all invoices for a specific customer
        [HttpGet("getInvByCusId/{customerid}")]
        public async Task<ActionResult<IEnumerable<InvoiceResponseModel>>> GetCustomerInvoices(int customerId)
        {
            var invoices = await _invoiceService.GetCustomerInvoicesAsync(customerId);
            return Ok(invoices);
        }

        // Endpoint to retrieve a list of customers
        [HttpGet("getcustomerscombodata")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomers()
        {
            try
            {
                var customers = await _invoiceService.GetAllCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customers");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

        // Endpoint to retrieve a list of products
        [HttpGet("getproductscombodata")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts()
        {
            try
            {
                var products = await _invoiceService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

    }
}
