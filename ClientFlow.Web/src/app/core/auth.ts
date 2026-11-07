import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    accessToken: string;
    expiresAt: string;
}

const TOKEN_KEY = 'cf_Token';
const EXPIRES_KEY = 'cf_expires';

@Injectable({ providedIn: 'root' })
export class Auth {
    private readonly http = inject(HttpClient);
    private readonly tokenState = signal<string | null>(this.readValidToken());

    readonly isLoggedIn = computed(() => this.tokenState() !== null);
    
    get token(): string | null {
        return this.tokenState();
    }

    login(request: LoginRequest): Observable<LoginResponse> {
        return this.http
        .post<LoginResponse>(`${environment.apiUrl}/Auth/Login`, request)
        .pipe(
            tap((response) => {
                localStorage.setItem(TOKEN_KEY, response.accessToken);
                localStorage.setItem(EXPIRES_KEY, response.expiresAt);
                this.tokenState.set(response.accessToken);
            }),
        );
    }

    logout(): void {
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(EXPIRES_KEY);
        this.tokenState.set(null);
    }

     private readValidToken(): string | null {
        const token = localStorage.getItem(TOKEN_KEY);
        const expires = localStorage.getItem(EXPIRES_KEY);
        if (!token || !expires || new Date(expires) <= new Date()) {
            return null;
        }
    return token;
  }
}
