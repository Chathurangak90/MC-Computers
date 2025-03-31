import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoiceCreateComponent } from './components/invoice-create/invoice-create.component';

const routes: Routes = [
  { path: '', redirectTo: '/invoice/create', pathMatch: 'full' },
  { path: 'invoice/create', component: InvoiceCreateComponent },];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
