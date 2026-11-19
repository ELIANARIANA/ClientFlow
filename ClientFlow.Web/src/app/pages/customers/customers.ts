import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CustomerApi } from '../../core/customer/customer-api';
import { HttpErrorResponse } from '@angular/common/http';
import { Customer, CustomerRequest } from '../../models/customer';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  imports: [DatePipe, RouterLink],
  selector: 'app-customers',
  styleUrl: './customers.scss',
  templateUrl: './customers.html',
})
export class Customers implements OnInit {
  private readonly api = inject(CustomerApi);

  readonly customers = signal<Customer[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly actionError = signal<string | null>(null);
  readonly deletingId = signal<string | null>(null);
  readonly search = signal('');

  readonly filtered = computed(() => {
    const term = this.search().trim().toLowerCase();
    const list = this.customers();
    console.log(term)
    if (!term) {
      return list;
    }
    return list.filter((c) =>
      [`${c.firstName} ${c.lastName}`, c.email, c.phone, c.companyName].some((value) =>
        value?.toLowerCase().includes(term),
      ),
    );
  });

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

  deleteCustomer(customer: Customer): void {
    const name = `${customer.firstName} ${customer.lastName}`;
    if (!confirm(`Customer "${name}" really be deleted?`)) {
      return;
    }

    this.actionError.set(null);
    this.deletingId.set(customer.customerId);

    this.api.deleteCustomer(customer.customerId).subscribe({
      next: () => {
        this.customers.update((list) =>
          list.filter((c) => c.customerId !== customer.customerId),
      );
      this.deletingId.set(null);
    },
    error: (err: HttpErrorResponse) => {
      this.deletingId.set(null);
      this.actionError.set(
        err.status === 404
        ? 'The customer no longer exists.'
        : 'The customer could not be deleted.'
      );
    },
   });
  }

   onSearch(event: Event): void {
      this.search.set((event.target as HTMLInputElement).value);
    }
}
