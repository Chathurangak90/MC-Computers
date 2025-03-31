import { InvoiceItem } from './InvoiceItemModel';

export interface Invoice {
    id?: number;
    customerId: number;
    transactionDate?: Date;
    totalAmount?: number;
    discountAmount?: number;
    balanceAmount?: number;
    items: InvoiceItem[];
  }