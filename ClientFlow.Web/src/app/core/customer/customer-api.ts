import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { Customer, CustomerRequest } from '../../models/customer';
import { environment } from '../../../environments/environment';

@Service()
export class CustomerApi {
    private readonly http = inject(HttpClient);

    getCustomers(): Observable<Customer[]> {
        return this.http.get<Customer[]>(`${environment.apiUrl}/Customers`);
    }

    getCustomer(customerId: string): Observable<Customer> {
        return this.http.get<Customer>(`${environment.apiUrl}/Customers/${customerId}`);
    }

    deleteCustomer(customerId: string): Observable<void> {
        return this.http.delete<void>(`${environment.apiUrl}/Customers/${customerId}`)
    }

    updateCustomer(customerId: string, request: CustomerRequest): Observable<void> {
        return this.http.put<void>(`${environment.apiUrl}/Customers/${customerId}`, request);
    }
}
