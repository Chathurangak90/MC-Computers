import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseConfig } from 'src/app/config/base-config';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {


  constructor(private http: HttpClient, private baseconfig: BaseConfig) {
  }

  // Method to create a new invoice
  createInvoice(invoiceData: any): Observable<any> {
    return this.http.post<any>(this.baseconfig.ApiBaseUrl+ '/api/invoice/create', invoiceData);
  }
//Load customer combodata
  loadCustomersComboData(): Observable<any> {
    const apiUrl =
      this.baseconfig.ApiBaseUrl + '/api/invoice/getCustomersComboData';
    return this.http.get<any>(apiUrl);
  }
//Load product combo data
  loadProductsComboData(): Observable<any> {
    const apiUrl =
      this.baseconfig.ApiBaseUrl + '/api/invoice/getProductsComboData';
    return this.http.get<any>(apiUrl);
  }

  // Method to get a single invoice by ID
  getInvoiceById(id: number): Observable<any> {
    const apiUrl = `${this.baseconfig.ApiBaseUrl}/api/invoice/getInvoiceById/${id}`;
    return this.http.get<any>(apiUrl);
  }
  

  // Method to get invoices by customer ID
  getCustomerInvoices(customerId: number): Observable<any[]> {
    const apiUrl = `${this.baseconfig.ApiBaseUrl}/api/invoice/getInvByCusId/${customerId}`;
    return this.http.get<any>(apiUrl);
  }
}
