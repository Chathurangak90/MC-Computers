import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Product } from 'src/app/models/ProductModel';
import { Customer } from 'src/app/models/CustomerModel';
import { Invoice } from 'src/app/models/InvoiceModel';
import { InvoiceService } from 'src/app/services/invoice.service';

@Component({
  selector: 'app-invoice-create',
  standalone: false,
  templateUrl: './invoice-create.component.html',
  styleUrl: './invoice-create.component.css'
})
export class InvoiceCreateComponent implements OnInit {
  invoiceForm: FormGroup; 
  products: Product[] = [];
  customers: Customer[] = [];
  currentTab: 'create' | 'details' = 'create';
  selectedInvoice: any = null;
  invoices: any[] = []; 
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private invoiceService: InvoiceService,
    private snackBar: MatSnackBar
  ) {
    // Initialize the invoice form
    this.invoiceForm = this.fb.group({
      customerId: [Validators.required], // Customer selection (Required)
      discountAmount: [0], // Default discount amount
      items: this.fb.array([]) // Form array to manage invoice items
    });
  }

  ngOnInit(): void {
    this.loadCustomers();
    this.loadProducts(); 
    this.addInvoiceItem();
  }

  // Fetch available products from the service
  loadProducts(): void {
    this.invoiceService.loadProductsComboData().subscribe(products => {
      this.products = products;
      // Set the first product as the default for existing invoice items
      if (this.products.length > 0) {
        this.invoiceItems.controls.forEach(control => {
          control.patchValue({ productId: this.products[0].id });
        });
      }
    });
  }

  // Fetch available customers from the service
  loadCustomers(): void {
    this.invoiceService.loadCustomersComboData().subscribe(customers => {
      this.customers = customers;
      // Auto-select the first customer if available
      if (this.customers.length > 0) {
        this.invoiceForm.patchValue({ customerId: this.customers[0].id });
      }
    });
  }

  // Getter method for accessing invoice items form array
  get invoiceItems() {
    return this.invoiceForm.get('items') as FormArray;
  }

  // Add a new invoice item row to the form array
  addInvoiceItem(): void {
    const itemForm = this.fb.group({
      productId: [this.products.length > 0 ? this.products[0].id : null, Validators.required],
      quantity: [0, [Validators.required, Validators.min(1)]] // Quantity (Must be at least 1)
    });
    this.invoiceItems.push(itemForm);
  }

  // Remove an invoice item row from the form array
  removeInvoiceItem(index: number): void {
    this.invoiceItems.removeAt(index);
  }

  // Handle form submission
  onSubmit(): void {
    this.invoiceForm.markAllAsTouched();

    const items = this.invoiceForm.get('items')?.value;
    
    // Ensure at least one item is present
    if (!items || items.length === 0) {
      this.snackBar.open('Please add at least one item to the invoice.', 'Close', { duration: 3000 });
      return;
    }

    // Check for duplicate products in the invoice items
    const productIds = items.map((item: { productId: any }) => item.productId);
    const duplicates = productIds.filter((item: any, index: number): boolean => productIds.indexOf(item) !== index);

    if (duplicates.length > 0) {
      this.snackBar.open('Cannot add the same product more than once.', 'Close', { duration: 3000 });
      return;
    }

    // Submit if form is valid
    if (this.invoiceForm.valid) {
      const invoiceData: Invoice = this.invoiceForm.value;
      this.isSubmitting = true;

      this.invoiceService.createInvoice(invoiceData).subscribe({
        next: () => {
          this.snackBar.open('Invoice created successfully!', 'Close', { duration: 500 });
          this.loadInvoices(); // Refresh invoice list
          this.invoiceItems.clear(); // Clear the form array
          this.addInvoiceItem(); // Add a new empty item
          this.loadProducts(); // Reload products
          this.isSubmitting = false;
          this.invoiceForm.patchValue({ discountAmount: 0 }); // Reset discount amount
        },
        error: (err) => {
          console.log(err);
          this.snackBar.open('Failed to create invoice', 'Close', { duration: 3000 });
        }
      });
    }
  }

  // Load invoices for a selected customer
  loadInvoices(): void {
    const customerId = this.customers[0].id;
    this.invoiceService.getCustomerInvoices(customerId).subscribe(
      invData => {
        this.invoices = invData;
        if (this.invoices.length > 0) {
          this.selectInvoice(this.invoices[0]);
        }
      },
      error => {
        console.error('Error loading invoices:', error);
      }
    );
  }

  // Select an invoice and switch to the details tab
  selectInvoice(invoice: any): void {
    this.selectedInvoice = invoice;
    this.currentTab = 'details';
  }

  // Switch back to the create tab
  goToCreateTab(): void {
    this.currentTab = 'create';
  }
}
