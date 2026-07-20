import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import {
  AuthTokenResponse,
  RequestMagicLinkRequest,
  RequestMagicLinkResponse,
  VerifyMagicLinkRequest,
} from '../types/auth.types';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private httpClient = inject(HttpClient);

  private readonly apiUrl = '/Auth';
  private readonly tokenStorageKey = 'clientportal_access_token';

  requestMagicLink(email: string): Observable<RequestMagicLinkResponse> {
    const body: RequestMagicLinkRequest = { email };
    return this.httpClient.post<RequestMagicLinkResponse>(`${this.apiUrl}/magic-link`, body);
  }

  verifyMagicLink(token: string): Observable<AuthTokenResponse> {
    const body: VerifyMagicLinkRequest = { token };
    return this.httpClient.post<AuthTokenResponse>(`${this.apiUrl}/verify`, body).pipe(
      tap((response) => sessionStorage.setItem(this.tokenStorageKey, response.accessToken)),
    );
  }

  getToken(): string | null {
    return sessionStorage.getItem(this.tokenStorageKey);
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }

  logout(): void {
    sessionStorage.removeItem(this.tokenStorageKey);
  }
}
