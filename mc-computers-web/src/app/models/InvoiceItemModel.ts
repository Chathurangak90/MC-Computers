export interface InvoiceItem {
    productId: number;
    productName?: string;
    quantity: number;
    unitPrice?: number;
    lineTotal?: number;
  }