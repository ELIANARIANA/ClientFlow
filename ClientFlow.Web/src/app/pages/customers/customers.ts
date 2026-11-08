import { Component, inject, OnInit, signal } from '@angular/core';
import { CustomerApi } from '../../core/customer/customer-api';
import { HttpErrorResponse } from '@angular/common/http';
import { Customer } from '../../models/customer';
import { DatePipe } from '@angular/common';

@Component({
  imports: [DatePipe],
  selector: 'app-customers',
  styleUrl: './customers.scss',
  templateUrl: './customers.html',
})
export class Customers implements OnInit {
  private readonly api = inject(CustomerApi);

  readonly customers = signal<Customer[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

    ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);

    this.api.getCustomers().subscribe({
      next: (data) => {
        this.customers.set(data);
        this.loading.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.loading.set(false);
        this.error.set(
          err.status === 401
          ? 'Session expired. Please log in again.'
          : 'Customers could not be loaded.',
        );
      },
    });
  }
}
