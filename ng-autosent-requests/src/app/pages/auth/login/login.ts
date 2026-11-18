import { inject } from '@angular/core';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth';
import { Router } from '@angular/router';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})
export class Login {

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loginForm = this.fb.nonNullable.group(
    {
      email: ['', [
        Validators.required,
        Validators.email,
      ]],
      password: ['', [
        Validators.required,
      ]],
    }
  )
  
  
  onLogin() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }
  
    this.authService.login(this.loginForm.getRawValue()).subscribe({
      next: (response) => {
        const { data, message, success } = response;
  
        if (success) {
          console.log(message);
  
          // Redirect after successful login
          this.router.navigate(['/dashboard']);
        } else {
          // Backend returned success: false
          console.error(message);
        }
      },
  
      error: (err) => {
        // HTTP errors such as 400, 401, 500
        console.error(
          'Login failed:',
          err.error?.message ?? 'Something went wrong'
        );
      }
    });
  }

}