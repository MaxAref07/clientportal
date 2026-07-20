import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private authService = inject(AuthService);
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);

  step = signal<'email' | 'token'>('email');
  isSubmitting = signal<boolean>(false);
  errorMessage = signal<string | null>(null);
  devToken = signal<string | null>(null);

  emailForm = this.formBuilder.group({
    email: this.formBuilder.control('', {
      validators: [Validators.required, Validators.email],
      nonNullable: true,
    }),
  });

  tokenForm = this.formBuilder.group({
    token: this.formBuilder.control('', {
      validators: [Validators.required],
      nonNullable: true,
    }),
  });

  requestLink() {
    if (this.emailForm.invalid) {
      this.emailForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    this.authService.requestMagicLink(this.emailForm.getRawValue().email).subscribe({
      next: (response) => {
        // In Development the backend returns the raw token — prefill it for manual testing.
        this.devToken.set(response.token);
        this.tokenForm.setValue({ token: response.token });
        this.step.set('token');
        this.isSubmitting.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to request a magic link. Please try again.');
        this.isSubmitting.set(false);
      },
    });
  }

  verify() {
    if (this.tokenForm.invalid) {
      this.tokenForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    this.authService.verifyMagicLink(this.tokenForm.getRawValue().token).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.router.navigate(['/']);
      },
      error: () => {
        this.errorMessage.set('Invalid or expired token. Please request a new link.');
        this.isSubmitting.set(false);
      },
    });
  }

  backToEmail() {
    this.step.set('email');
    this.errorMessage.set(null);
    this.devToken.set(null);
    this.tokenForm.reset({ token: '' });
  }
}
