import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { Customer } from '../../models/customer';
import { environment } from '../../../environments/environment';

@Service()
export class CustomerApi {
    private readonly http = inject(HttpClient);

    getCustomers(): Observable<Customer[]> {
        return this.http.get<Customer[]>(`${environment.apiUrl}/Customers`);
    }
}
