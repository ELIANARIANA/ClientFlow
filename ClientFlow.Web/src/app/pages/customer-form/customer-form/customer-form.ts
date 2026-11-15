import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomerApi } from '../../../core/customer/customer-api';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CustomerRequest } from '../../../models/customer';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
 imports: [ReactiveFormsModule, RouterLink],
  selector: 'app-customer-form',
  styleUrl: './customer-form.scss',
  templateUrl: './customer-form.html',
})
export class CustomerForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(CustomerApi);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly customerId = this.route.snapshot.paramMap.get('id')!;

  readonly loading = signal(true);
  readonly saving = signal(false);
  readonly error = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required]],
    phone: ['', [Validators.required]],
    companyName: ['', [Validators.required]],
  });

  ngOnInit(): void {
    this.api.getCustomer(this.customerId).subscribe({
      next: (customer) => {
        this.form.patchValue({
          firstName: customer.firstName,
          lastName: customer.lastName,
          email: customer.email,
          phone: customer.phone ?? '',
          companyName: customer.companyName ?? '',
        });
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.error.set('The customer could not be loaded.')
      }
    });
  }

  save(): void {
    if (this.form.invalid || this.saving()) {
      this.form.markAllAsTouched();
      return;
    }
    
    const value = this.form.getRawValue();
    const request: CustomerRequest = {
      firstName: value.firstName.trim(),
      lastName: value.lastName.trim(),
      email: value.email.trim(),
      phone: value.phone.trim() || null,
      companyName: value.companyName.trim() || null,
    };
    
    this.saving.set(true);
    this.error.set(null);
    
    this.api.updateCustomer(this.customerId, request).subscribe({
      next: () => this.router.navigateByUrl('/customers'),
      error: (err: HttpErrorResponse) => {
        this.saving.set(false);
        this.error.set(
          err.status === 400
            ? 'Your input was rejected by the server. Please check.'
            : err.status === 404
              ? 'The customer no longer exists..'
              : 'Saving failed.',
        );
      },
    });
  }
}
