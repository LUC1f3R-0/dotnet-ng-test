import { inject } from '@angular/core';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})
export class Login {

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);

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
    if (this.loginForm.invalid)
    {
      this.loginForm.markAllAsTouched();
      return;  
    }
    console.log(this.loginForm.getRawValue());

    this.authService.login(this.loginForm.getRawValue()).subscribe({
      next: (response) => {
        console.log(response)
      },
      error: (err) => {
        
      },
      complete: () => {
        
      }
    })
  }
}